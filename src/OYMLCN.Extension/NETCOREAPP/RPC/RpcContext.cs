using Microsoft.AspNetCore.Http;
using OYMLCN.Extensions;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 过程调用委托执行方法
    /// </summary>
    public delegate Task RpcRequestDelegate(RpcContext context);

    /// <summary>
    /// 过程调用上下文信息
    /// </summary>
    public class RpcContext
    {
        /// <summary>
        /// 过程调用目标方法允许匿名调用
        /// </summary>
        public bool MethodAllowAnonymous
        {
            get
            {
                var methodAttribute = Method?.GetAttribute<RpcAuthorizeAttribute>();
                var targetAttribute = TargetType?.GetAttribute<RpcAuthorizeAttribute>();
                if (methodAttribute == null && targetAttribute == null) return true;
                return methodAttribute != null && methodAttribute.AllowAnonymous;
            }
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestBody { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object ReturnValue { get; set; }
        /// <summary>
        /// 调用参数
        /// </summary>
        public object[] Parameters { get; set; }
        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TargetType { get; set; }
        /// <summary>
        /// 调用方法
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// 请求上下文
        /// </summary>
        public HttpContext HttpContext { get; set; }
        /// <summary>
        /// 任务处理计时器
        /// </summary>
        public Stopwatch Stopwatch { get; internal set; } = new Stopwatch();
    }
}
