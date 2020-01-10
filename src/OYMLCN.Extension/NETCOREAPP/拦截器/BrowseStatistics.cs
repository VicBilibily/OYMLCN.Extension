using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using OYMLCN.AspNetCore;
using OYMLCN.Extensions;

namespace OYMLCN.AspNetCore
{
    /// <summary>
    /// 浏览分析结果处理
    /// </summary>
    public delegate Task BrowseStatisticsDelegate(ActionContext context, BrowseStatisticsResult result);
    /// <summary>
    /// 浏览分析配置信息
    /// </summary>
    public class BrowseStatisticsOptions
    {
        internal BrowseStatisticsDelegate handlerDelegate { get; set; }
    }
    /// <summary>
    /// 浏览分析结果信息
    /// </summary>
    public class BrowseStatisticsResult
    {
#pragma warning disable 1591
        internal BrowseStatisticsResult() { }

        public string RequestUrl => $"{RequestScheme}://{RequestHost}{RequestPath}{RequestQueryString}";
        public string RequestMethod { get; internal set; }
        public string RequestScheme { get; internal set; }
        public HostString RequestHost { get; internal set; }
        public string RequestPath { get; internal set; }
        public QueryString RequestQueryString { get; internal set; }
        public string RequestUserAgent { get; internal set; }

        private UAParser _UAInfo;
        public UAParser BrowserInfo => _UAInfo ?? (_UAInfo = UAParser.Parse(RequestUserAgent));

        public string AreaName { get; internal set; }
        public string ControllerName { get; internal set; }
        public string ActionName { get; internal set; }
        public IDictionary<string, object> ActionArguments { get; internal set; }

        public string FilterRemark { get; set; }
#pragma warning restore 1591
    }

    /// <summary>
    /// 浏览分析过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class BrowseStatisticsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 不处理分析当前拦截
        /// </summary>
        public bool Ignore { get; set; } = false;
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// Action 执行之前，数据模型绑定已完成
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var filters = context.Filters
                .Where(v => v.GetType() == typeof(BrowseStatisticsAttribute))
                .Select(v => v as BrowseStatisticsAttribute)
                .OrderBy(v => v.Order)
                .ToList();
            // 忽略标记为跳过的方法或控制器
            if (filters.Any(v => v.Ignore)) return;
            // 如果同时存在多个，则只执行最后一个
            if (filters.Last() != this) return;
            // 合并最终的处理参数设置
            if (filters.Count > 1)
            {
                if (this.Remark == null)
                    this.Remark = filters.Select(v => v.Remark)
                        .Where(v => v.IsNotNullOrEmpty()).LastOrDefault();
                // ：TODO
            }

            // 尝试获取已注入的分析配置
            var options = context.GetService<IOptions<BrowseStatisticsOptions>>()?.Value;

            if (options != null && options.handlerDelegate != null)
            {
                var result = new BrowseStatisticsResult();

                var requset = context.HttpContext.Request;
                result.RequestMethod = requset.Method;
                result.RequestScheme = requset.Scheme;
                result.RequestHost = requset.Host;
                result.RequestPath = requset.Path;
                result.RequestQueryString = requset.QueryString;
                result.RequestUserAgent = requset.Headers[HeaderNames.UserAgent];

                result.AreaName = context.ActionDescriptor.RouteValues["area"] as string;
                result.ControllerName = context.ActionDescriptor.RouteValues["controller"] as string;
                result.ActionName = context.ActionDescriptor.RouteValues["action"] as string;
                result.ActionArguments = context.ActionArguments;

                result.FilterRemark = this.Remark;

                options.handlerDelegate(context, result);
                //var task = options.handlerDelegate(context, result);
                //if (!task.IsCompletedSuccessfully)
                //{
                //    //:TODO 未能成功执行完毕的错误处理
                //}
            }
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
        /// 添加使用浏览分析器
        /// </summary>
        public static IServiceCollection AddBrowseStatistics(this IServiceCollection builder)
        {
            builder.Configure(new Action<BrowseStatisticsOptions>(_ => { }));
            return builder;
        }
        /// <summary>
        /// 添加使用浏览分析器
        /// </summary>
        public static IServiceCollection AddBrowseStatistics(this IServiceCollection builder, Action<BrowseStatisticsOptions> configure)
        {
            builder.Configure(configure);
            return builder;
        }
        /// <summary>
        /// 使用浏览分析器逻辑
        /// </summary>
        public static IApplicationBuilder UseBrowseStatistics(this IApplicationBuilder app, BrowseStatisticsDelegate handler)
        {
            var options = app.GetRequiredService<IOptions<BrowseStatisticsOptions>>().Value;
            options.handlerDelegate = handler;
            return app;
        }
    }
}

