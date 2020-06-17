using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using OYMLCN.Extensions;
using System.Net;
using OYMLCN.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// StartupConfigureExtension
    /// </summary>
    public static partial class StartupConfigureExtensions
    {
#if NETCOREAPP3_1
        /// <summary>
        /// 添加字符转化器，以避免中文被编码
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHtmlEncoder(this IServiceCollection services)
            => services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
        /// <summary>
        /// 修正 System.Text.Json 表现与 Newtonsoft.Json 表现不一致的问题
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <returns></returns>
        public static IMvcBuilder FixJsonOptionsUnsafe(this IMvcBuilder mvcBuilder)
        {
            var opts = JsonExtensions.JsonOptions;
            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = opts.Encoder;
                // 前端对象绑定需要有定义才能双向绑定，故不再忽略空值
                //options.JsonSerializerOptions.IgnoreNullValues = opts.IgnoreNullValues;
            });
            return mvcBuilder;
        }
#endif
    }
}
namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// StartupConfigureExtension
    /// </summary>
    public static partial class StartupConfigureExtensions
    {
        /// <summary>
        /// 开启腾讯CDN加速请求头识别
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseQcloudForwardedHeaders(this IApplicationBuilder app) =>
              app.UseForwardedHeaders(new ForwardedHeadersOptions
              {
                  ForwardedForHeaderName = "X-Forwarded-For",
                  ForwardedProtoHeaderName = "X-Forwarded-Proto",
                  OriginalForHeaderName = "X-Original-For",
                  OriginalProtoHeaderName = "X-Original-Proto",
                  ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
              });

        /// <summary>
        /// 返回异常Json数据信息，适用于纯接口服务
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJsonResultForStatusCodeAndException(this IApplicationBuilder app) =>
              app.UseStatusCodePages(context =>
              {
                  var response = context.HttpContext.Response;
                  var statusCode = response.StatusCode;
                  response.StatusCode = 200;
                  response.ContentType = "application/json";
                  return response.WriteAsync(new
                  {
                      code = statusCode,
                      msg = $"{statusCode} {(HttpStatusCode)statusCode}"
                  }.JsonSerialize(), Encoding.UTF8);
              }).UseExceptionHandler(config =>
              {
                  config.Run(handler =>
                  {
                      var err = handler.Features.Get<IExceptionHandlerFeature>();
                      handler.Response.StatusCode = 200;
                      handler.Response.ContentType = "application/json";
                      return handler.Response.WriteAsync(new
                      {
                          code = 500,
                          msg = err.Error.Message
                      }.JsonSerialize(), Encoding.UTF8);
                  });
              });

    }
}