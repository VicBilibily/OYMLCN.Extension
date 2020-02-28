using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OYMLCN.AspNetCore;
using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace OYMLCN.AspNetCore
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        string ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// Jwt配置信息
    /// </summary>
    public sealed class JwtOptions
    {
        /// <summary>
        /// AccessToken名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = "access_token";

        /// <summary>
        /// 签名密钥
        /// </summary>
        [JsonProperty("secret")]
        public string Secret { get; set; }
        /// <summary>
        /// 签名密钥
        /// </summary>
        internal SecurityKey SigningKey
        {
            get
            {
                var key = Secret.GetUTF8Bytes();
                // 确认加密密钥是否是 32 位，不是的话用字符串 MD5 的HASH值
                if (key.Length != 32)
                    key = Secret.HashToMD5().GetUTF8Bytes();
                return new SymmetricSecurityKey(key);
            }
    } 

        /// <summary>
        /// 签名发行标识
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        /// <summary>
        /// 签名目标用户
        /// </summary>
        [JsonProperty("audience")]
        public string Audience { get; set; }

        /// <summary>
        /// Token有效期（单位：分钟）[默认：30]
        /// </summary>
        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; } = 30;
        /// <summary>
        /// Refresh有效期（单位：分钟）[默认：60]
        /// </summary>
        [JsonProperty("refreshExpiration")]
        public int RefreshExpiration { get; set; } = 60;
        /// <summary>
        /// 时钟漂移（允许的时间误差）（单位：秒）[默认：300]
        /// </summary>
        [JsonProperty("clockSkew")]
        public int ClockSkew { get; set; } = 300;
    }
    /// <summary>
    /// JwtToken
    /// </summary>
    public sealed class JwtToken
    {
        internal JwtToken(JwtSecurityToken token, string tokenName, int expires)
        {
            this.token_name = tokenName;
            this.access_token = new JwtSecurityTokenHandler().WriteToken(token);
            this.refresh_token = Guid.NewGuid().ToString("N");
            this.expires_in = expires * 60;
        }
        /// <summary>
        /// 凭证名称（默认应为access_token）
        /// </summary>
        public string token_name { get; }
        /// <summary>
        /// 凭证内容
        /// </summary>
        public string access_token { get; }
        /// <summary>
        /// 凭证刷新
        /// </summary>
        public string refresh_token { get; }
        /// <summary>
        /// 过期时间（相对N秒后）
        /// </summary>
        public int expires_in { get; }

        private Dictionary<string, object> Data
        {
            get
            {
                var dic = new Dictionary<string, object>();
                dic.Add(token_name, access_token);
                dic.Add(nameof(refresh_token), refresh_token);
                dic.Add(nameof(expires_in), expires_in);
                return dic;
            }
        }
        /// <summary>
        /// 转换为Json
        /// </summary>
        public string ToJsonString()
            => Data.ToJsonString();
        /// <summary>
        /// 显式转换为string
        /// </summary>
        public static explicit operator string(JwtToken jwtToken)
            => jwtToken.ToJsonString();
        /// <summary>
        /// 隐式转换为JsonResult
        /// </summary>
        public static implicit operator JsonResult(JwtToken jwtToken)
            => new JsonResult(jwtToken.Data);
    }

    /// <summary>
    /// ControllerExtension
    /// </summary>
    public static partial class ControllerExtension
    {
        /// <summary>
        /// 传入用户信息构建JwtSecurityToken
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static JwtToken BuildJwtSecurityToken(this Controller controller, IUserInfo userInfo)
            => controller.BuildJwtSecurityToken(userInfo.ToJsonString());
        private static JwtToken BuildJwtSecurityToken(this Controller controller, string json)
        {
            var options = controller.GetRequiredOptions<JwtOptions>();

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

            var result = new JwtToken(jwtToken, options.Name, options.AccessExpiration);

            var MemoryCache = controller.GetRequiredService<IMemoryCache>();
            var exp = TimeSpan.FromMinutes(options.RefreshExpiration);
            MemoryCache.Set($"_jwt_refresh_token_{result.refresh_token}", md5, exp);
            MemoryCache.Set($"_jwt_created_token_{md5}", json, exp);
            return result;
        }

        /// <summary>
        /// 传入refresh_token构建JwtSecurityToken
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public static JwtToken RefreshJwtSecurityToken(this Controller controller, string refresh_token)
        {
            var MemoryCache = controller.GetRequiredService<IMemoryCache>();
            var refreshTokenKey = $"_jwt_refresh_token_{refresh_token}";
            if (MemoryCache.TryGetValue(refreshTokenKey, out string md5))
            {
                var cacheTokenMD5 = $"_jwt_created_token_{md5}";
                if (MemoryCache.TryGetValue(cacheTokenMD5, out string json))
                {
                    // 刷新凭证已经使用，移除刷新凭据
                    MemoryCache.Remove(refreshTokenKey);
                    return controller.BuildJwtSecurityToken(json);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取登录用户的信息（如果未登录则返回null）
        /// </summary>
        public static T GetUserInfo<T>(this Controller controller) where T : IUserInfo
        {
            if (controller.IsAuthenticated)
            {
                var info = controller.User.Claims.FirstOrDefault(v => v.Type == "aes")?.Value;
                if (info != null)
                {
                    var options = controller.GetRequiredOptions<JwtOptions>();
                    info = info.AESDecrypt(options.Secret.HashToMD5());
                    return info.DeserializeJsonToObject<T>();
                }
            }
            return default(T);
        }
    }
}

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// StartupConfigureExtension
    /// </summary>
    public static partial class StartupConfigureExtension
    {
        /// <summary>
        /// 配置JsonWebToken(JWT)身份验证
        /// <para>需要在配置文件设置 jwt -> [name] / secret / issuer / audience / [accessExpiration] / [refreshExpiration]</para>
        /// <para>需在Configure中加入 app.UseAuthentication() 以使得登陆配置生效</para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfgKey">配置节名称，默认为jwt</param>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string cfgKey = "jwt")
        {
            services.AddMemoryCache();
            var Configuration = services.GetRequiredService<IConfiguration>();
            services.Configure<JwtOptions>(Configuration.GetSection(cfgKey));
            var jwtOptions = services.GetRequiredOptions<JwtOptions>();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    //options.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            //context.Request.Path.StartsWithSegments("/api");
                            //context.HandleResponse();
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {

                            return Task.CompletedTask;
                        },
                        // 注册自定义token获取方式
                        OnMessageReceived = context =>
                        {
                            // 首先尝试从Cookie中获取Token
                            string token = context.Request.Cookies[jwtOptions.Name];
                            // 如果无，则尝试参数从中获取Token
                            if (token.IsNullOrEmpty()) token = context.Request.Query[jwtOptions.Name];
                            // 执行完毕，把取得的值设置为token
                            // 如果为空原始方式会从Header重新获取
                            context.Token = token;
                            return Task.CompletedTask;
                        }
                    };
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtOptions.SigningKey,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(jwtOptions.ClockSkew)
                    };
                });
            return services;
        }
    }


}
