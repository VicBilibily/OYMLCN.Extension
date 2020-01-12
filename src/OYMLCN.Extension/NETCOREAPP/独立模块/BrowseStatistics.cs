using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
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
        /// <summary>
        /// 启动统计拦截（可在调试时通过条件配置文件禁用统计）
        /// </summary>
        public bool Disabled { get; set; } = false;
        /// <summary>
        /// 只统计GET请求，默认值false
        /// </summary>
        public bool OnlyGetMethod { get; set; } = false;
        internal BrowseStatisticsDelegate handlerDelegate { get; set; }
    }
    /// <summary>
    /// 浏览分析结果信息
    /// </summary>
    public class BrowseStatisticsResult
    {
#pragma warning disable 1591
        internal BrowseStatisticsResult() { }

        public string RequestUrl => $"{RequestScheme}://{RequestHost}{RequestPath}";
        public string RequestQueryUrl => $"{RequestUrl}{RequestQueryString}";
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

        public string FilterName { get; internal set; }
        public string FilterRemark { get; internal set; }

        /// <summary>
        /// 新独立IP访客
        /// </summary>
        public bool IsNewUV { get; internal set; }
        /// <summary>
        /// 新的登录用户
        /// </summary>
        public bool IsNewLogin { get; internal set; }
#pragma warning restore 1591
    }

    /// <summary>
    /// 浏览分析过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class BrowseStatisticsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 自定义处理名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 不处理分析当前拦截
        /// </summary>
        public bool Ignore { get; set; } = false;
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 只统计指定参数名称（多个以 , 号分割）
        /// </summary>
        public string Arguments { get; set; }
        /// <summary>
        /// 忽略指定参数名称（多个以 , 号分割）[all则全部忽略]
        /// </summary>
        public string RemoveArguments { get; set; }


        /// <summary>
        /// Action 执行之前，数据模型绑定已完成
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 尝试获取已注入的分析配置
            var options = context.GetRequiredOptions<BrowseStatisticsOptions>();
            // 获取注入的内存缓存
            var MemoryCache = context.GetService<IMemoryCache>();
            // 已禁用统计的情况下不再继续
            if (options.Disabled) return;
            // 如果只统计GET请求，不是GET的全部忽略
            if ( options.OnlyGetMethod && context.HttpContext.Request.Method != HttpMethod.Get.Method)
                return;

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
                if (this.Name == null)
                    this.Name = filters.Select(v => v.Name)
                        .Where(v => v.IsNotNullOrEmpty()).LastOrDefault();
                if (this.Remark == null)
                    this.Remark = filters.Select(v => v.Remark)
                        .Where(v => v.IsNotNullOrEmpty()).LastOrDefault();
                this.Arguments = filters.Select(v => v.Arguments).Where(v => v.IsNotNullOrEmpty()).Join(",");
                this.RemoveArguments = filters.Select(v => v.RemoveArguments).Where(v => v.IsNotNullOrEmpty()).Join(",");
            }

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

                var ActionArguments = context.ActionArguments.ToDictionary(v => v.Key, v => v.Value);
                if (this.Arguments.IsNotNullOrEmpty())
                {
                    var paramArr = this.Arguments.SplitAuto().Select(v => v.Trim().ToLower()).ToArray();
                    ActionArguments = ActionArguments.Where(v => paramArr.Contains(v.Key.ToLower())).ToDictionary(v => v.Key, v => v.Value);
                }
                if (this.RemoveArguments.IsNotNullOrEmpty())
                {
                    if (this.RemoveArguments.ToLower() == "all")
                        ActionArguments = new Dictionary<string, object>();
                    else
                    {
                        var paramArr = this.RemoveArguments.SplitAuto().Select(v => v.Trim().ToLower()).ToArray();
                        ActionArguments = ActionArguments.Where(v => !paramArr.Contains(v.Key.ToLower())).ToDictionary(v => v.Key, v => v.Value);
                    }
                }
                result.ActionArguments = ActionArguments;

                result.FilterName = this.Name;
                result.FilterRemark = this.Remark;

                var controller = context.Controller as Controller;
                MemoryCache.GetOrCreate($"_bs_ip_{controller.RequestSourceIP}", (cache) =>
                {
                    result.IsNewUV = true;
                    cache.SetValue(true);
                    cache.AbsoluteExpiration = DateTime.Now.GetNextDayStart();
                    return true;
                });
                if (controller?.IsAuthenticated == true)
                    MemoryCache.GetOrCreate($"_bs_nl_{controller.UserId}_{controller.UserName}", (cache) =>
                    {
                        result.IsNewLogin = true;
                        cache.SetValue(true);
                        cache.AbsoluteExpiration = DateTime.Now.GetNextDayStart();
                        return true;
                    });

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
            => builder.AddBrowseStatistics(_ => { });
        /// <summary>
        /// 添加使用浏览分析器
        /// </summary>
        public static IServiceCollection AddBrowseStatistics(this IServiceCollection builder, Action<BrowseStatisticsOptions> configure)
        {
            builder.AddMemoryCache();
            builder.Configure(configure);
            return builder;
        }
        /// <summary>
        /// 使用浏览分析器逻辑
        /// </summary>
        public static IApplicationBuilder UseBrowseStatistics(this IApplicationBuilder app, BrowseStatisticsDelegate handler)
        {
            var options = app.GetRequiredOptions<BrowseStatisticsOptions>();
            options.handlerDelegate = handler;
            return app;
        }
    }
}

