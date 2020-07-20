﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OYMLCN.AspNetCore;
using OYMLCN.AspNetCore.TransferJob;
using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace OYMLCN.RPC.Server
{
    /// <summary>
    /// 远程调用辅助类
    /// </summary>
    public partial class RpcHelper
    {
        /// <summary>
        /// 注入服务提供器
        /// </summary>
        public IServiceProvider ServiceProvider { get; }
        /// <summary>
        /// 远程调用辅助类
        /// </summary>
        public RpcHelper(IServiceProvider requestServices)
        {
            this.ServiceProvider = requestServices;
        }

        private IWebHostEnvironment _hostEnvironment;
        private IConfiguration _configuration;
        private IMemoryCache _memoryCache;
        private IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpContext;

        private IBackgroundRunService _backgroundRunService;

        /// <summary>
        /// 主机环境信息
        /// </summary>
        public IWebHostEnvironment HostEnvironment
            => _hostEnvironment ??= ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        /// <summary>
        /// 程序运行配置
        /// </summary>
        public IConfiguration Configuration
           => _configuration ??= ServiceProvider.GetRequiredService<IConfiguration>();
        /// <summary>
        /// 内存缓存实例
        /// </summary>
        public IMemoryCache MemoryCache
            => _memoryCache ??= ServiceProvider.GetRequiredService<IMemoryCache>();
        private IHttpContextAccessor HttpContextAccessor
            => _httpContextAccessor ??= ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        /// <summary>
        /// 请求上下文信息
        /// </summary>
        public HttpContext HttpContext
            => _httpContext ??= HttpContextAccessor.HttpContext;

        /// <summary>
        /// 后台任务提供器
        /// </summary>
        public IBackgroundRunService BackgroundRunService
           => _backgroundRunService ??= ServiceProvider.GetRequiredService<IBackgroundRunService>();

        #region 用户身份认证
        /// <summary>
        /// 创建用户认证Token
        /// </summary>
        /// <param name="options"> JWT 配置信息 </param>
        /// <param name="data"> 用户认证数据 </param>
        /// <returns> TokenString </returns>
        public string CreateToken<T>(JwtOptions options, T data) where T : class, new()
        {
            var json = data.JsonSerialize();
            var md5 = json.HashToMD5();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, md5),
                new Claim("aes",json.AESEncrypt(options.Secret.HashToMD5()))
            };

            DateTime? expires = null;
            if (options.AccessExpiration > 0)
                expires = DateTime.UtcNow.AddMinutes(options.AccessExpiration);

            var credentials = new SigningCredentials(options.SigningKey, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(options.Issuer, options.Audience, claims, expires: expires, signingCredentials: credentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var token = tokenString.Split('.').TakeLast(2).Join("."); ;
            MemoryCache.Set($"tokenCache_{token}", md5, TimeSpan.FromMinutes(options.AccessExpiration));
            MemoryCache.Set($"tokenCacheData_{token}", data, TimeSpan.FromMinutes(options.AccessExpiration));
            return token;
        }
        /// <summary>
        /// 验证用户认证Token是否有效
        /// </summary>
        /// <param name="options"> JWT 配置信息 </param>
        public bool ValidateToken(JwtOptions options)
            => this.ValidateToken(options, RpcRequest.Token);
        /// <summary>
        /// 验证用户认证Token是否有效
        /// </summary>
        /// <param name="options"> JWT 配置信息 </param>
        /// <param name="token"> 用户认证数据 </param>
        public bool ValidateToken(JwtOptions options, string token)
        {
            if (token.IsNullOrWhiteSpace()) return false;

            if (MemoryCache.TryGetValue($"tokenCache_{token}", out _))
                return true;

            var tokenHandler = new JwtSecurityTokenHandler();
            var fullToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." + token.Split('.').TakeLast(2).Join(".");

            var jwtSecurityToken = tokenHandler.ReadJwtToken(fullToken);
            var claims = jwtSecurityToken.Claims;
            try
            {
                tokenHandler.ValidateToken(fullToken, new TokenValidationParameters()
                {
                    ValidAudiences = jwtSecurityToken.Audiences,
                    ValidIssuer = options.Issuer,
                    IssuerSigningKey = options.SigningKey,
                }, out SecurityToken validatedToken);
            }
            catch { return false; }

            int.TryParse(claims.FirstOrDefault(v => v.Type == "exp").Value, out int timestamp);
            if (DateTime.UtcNow.ToTimestampInt64() - timestamp > options.AccessExpiration * 60)
                return false;

            var md5 = claims.FirstOrDefault(v => v.Type == JwtRegisteredClaimNames.Jti)?.Value;
            MemoryCache.Set($"tokenCache_{token}", md5, TimeSpan.FromSeconds(timestamp - DateTime.UtcNow.ToTimestampInt64()));
            return true;
        }
        /// <summary>
        /// 从 Token 获取用户认证数据
        /// </summary>
        /// <param name="options"> JWT 配置信息 </param>
        /// <returns> 如果 token 是由服务颁发的有效凭证且未过期，则会返回生成时提供的用户认证数据信息，否则返回 null。 </returns>
        public T GetTokenInfo<T>(JwtOptions options) where T : class, new()
            => this.GetTokenInfo<T>(options, RpcRequest.Token);
        /// <summary>
        /// 从 Token 获取用户认证数据
        /// </summary>
        /// <param name="options"> JWT 配置信息 </param>
        /// <param name="token"> token </param>
        /// <returns> 如果 token 是由服务颁发的有效凭证且未过期，则会返回生成时提供的用户认证数据信息，否则返回 null。 </returns>
        public T GetTokenInfo<T>(JwtOptions options, string token) where T : class, new()
        {
            if (token.IsNullOrWhiteSpace()) return default;
            if (!ValidateToken(options, token)) return default;
            if (MemoryCache.TryGetValue($"tokenCacheData_{token}", out T info))
                return info;

            var fullToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." + token.Split('.').TakeLast(2).Join(".");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(fullToken);
            var claims = jwtSecurityToken.Claims;

            var jti = claims.FirstOrDefault(v => v.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var aes = claims.FirstOrDefault(v => v.Type == "aes")?.Value;

            if (jti.IsNullOrWhiteSpace() || aes.IsNullOrWhiteSpace()) return default;
            try
            {
                info = aes.AESDecrypt(options.Secret.HashToMD5()).JsonDeserialize<T>();
                int.TryParse(claims.FirstOrDefault(v => v.Type == "exp").Value, out int timestamp);
                MemoryCache.Set($"tokenCacheData_{token}", info, TimeSpan.FromSeconds(timestamp - DateTime.UtcNow.ToTimestampInt64()));
                return info;
            }
            catch { return default; }
        }
        #endregion

        /// <summary>
        /// 获取或创建缓存数据（默认缓存10s）
        /// </summary>
        /// <param name="key"> 缓存键 </param>
        /// <param name="valueFactory"> 数据工厂 </param>
        /// <param name="expSeconds"> 缓存时间（默认10s） </param>
        public T GetOrAddCache<T>(string key, Func<T> valueFactory, int expSeconds = 10)
            => MemoryCache.GetOrCreate(key, cache =>
            {
                var value = valueFactory.Invoke();
                cache.SetAbsoluteExpiration(TimeSpan.FromSeconds(expSeconds))
                     .SetValue(value);
                return value;
            });


    }
}