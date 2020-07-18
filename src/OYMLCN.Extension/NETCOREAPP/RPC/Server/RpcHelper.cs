using Microsoft.AspNetCore.Hosting;
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
        public IServiceProvider RequestServices { get; }
        /// <summary>
        /// 远程调用辅助类
        /// </summary>
        public RpcHelper(IServiceProvider requestServices)
        {
            this.RequestServices = requestServices;
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
            => _hostEnvironment ??= RequestServices.GetRequiredService<IWebHostEnvironment>();
        /// <summary>
        /// 程序运行配置
        /// </summary>
        public IConfiguration Configuration
           => _configuration ??= RequestServices.GetRequiredService<IConfiguration>();
        /// <summary>
        /// 内存缓存实例
        /// </summary>
        public IMemoryCache MemoryCache
            => _memoryCache ??= RequestServices.GetRequiredService<IMemoryCache>();
        private IHttpContextAccessor HttpContextAccessor
            => _httpContextAccessor ??= RequestServices.GetRequiredService<IHttpContextAccessor>();
        /// <summary>
        /// 请求上下文信息
        /// </summary>
        public HttpContext HttpContext
            => _httpContext ??= HttpContextAccessor.HttpContext;

        /// <summary>
        /// 后台任务提供器
        /// </summary>
        public IBackgroundRunService BackgroundRunService
           => _backgroundRunService ??= RequestServices.GetRequiredService<IBackgroundRunService>();


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

            return tokenString.Split('.').TakeLast(2).Join(".");
        }
        /// <summary>
        /// 从 Token 获取用户认证数据
        /// </summary>
        /// <param name="options"> JWT 配置信息 </param>
        /// <param name="token"> token </param>
        /// <returns> 如果 token 是由服务颁发的有效凭证且未过期，则会返回生成时提供的用户认证数据信息，否则返回 null。 </returns>
        public T GetTokenInfo<T>(JwtOptions options, string token) where T : class, new()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." + token.Split('.').TakeLast(2).Join(".");

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims;
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidAudiences = jwtSecurityToken.Audiences,
                    ValidIssuer = options.Issuer,
                    IssuerSigningKey = options.SigningKey,
                }, out SecurityToken validatedToken);
            }
            catch { return default; }

            int.TryParse(claims.FirstOrDefault(v => v.Type == "exp").Value, out int timestamp);
            if (DateTime.Now.ToTimestampInt64() - timestamp > options.AccessExpiration * 60)
                return default;

            var jti = claims.FirstOrDefault(v => v.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var aes = claims.FirstOrDefault(v => v.Type == "aes")?.Value;

            if (jti.IsNullOrWhiteSpace() || aes.IsNullOrWhiteSpace()) return default;
            try { return aes.AESDecrypt(options.Secret.HashToMD5()).JsonDeserialize<T>(); }
            catch { return default; }
        } 
        #endregion


    }
}
