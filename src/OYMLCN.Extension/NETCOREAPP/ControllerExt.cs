using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OYMLCN.AspNetCore;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// ControllerExtension
    /// </summary>
    public static partial class ControllerExtension
    {
        /// <summary>
        /// 用户登陆（将已经生成的JWT身份认证信息写入到Cookie）
        /// 若要使用Cookie+JWT验证，需要在Startup中配置app.UseJWTAuthenticationWithCookie
        /// </summary>
        public static void SignInJwt(this Controller controller, JwtToken jwt, bool secureCookie = true, CookieOptions options = null)
        {
            options = options ?? options ?? new CookieOptions();
            options.Secure = secureCookie;
            options.HttpOnly = true;
            controller.HttpContext.Response.Cookies.Append(jwt.token_name, jwt.access_token, options);
        }

        /// <summary>
        /// 注销登陆（删除Cookie中的JWT身份凭证）
        /// tokenKey 为 null 时将会尝试从配置文件 JWT:Name 中获取
        /// </summary>
        public static void SignOutJwt(this Controller controller, string tokenKey = null, bool secureCookie = true, CookieOptions options = null)
        {
            options = options ?? new CookieOptions();
            options.Secure = secureCookie;
            options.HttpOnly = true;

            if (tokenKey.IsNullOrEmpty())
                tokenKey = controller.GetRequiredOptions<JwtOptions>().Name;
            controller.HttpContext.Response.Cookies.Delete(tokenKey, options);
        }

    }
}
