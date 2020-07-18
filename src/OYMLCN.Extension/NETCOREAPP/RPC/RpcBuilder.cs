using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OYMLCN.RPC.Core.RpcBuilder
{
    /// <summary>
    /// 过程调用上下文信息
    /// </summary>
    public class RpcContext
    {
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

    /// <summary>
    /// 过程调用委托执行方法
    /// </summary>
    public delegate Task RpcRequestDelegate(RpcContext context);

    /// <summary>
    /// 过程调用触发器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class RpcFilterAttribute : Attribute
    {
        /// <summary>
        /// 触发方法定义
        /// </summary>
        public abstract Task InvokeAsync(RpcContext context, RpcRequestDelegate next);
    }

    /// <summary>
    /// 过程调用管道工厂
    /// </summary>
    public class TaskPiplineBuilder
    {
        private readonly IList<Func<RpcRequestDelegate, RpcRequestDelegate>> _components;

        /// <summary>
        /// 过程调用管道工厂
        /// </summary>
        public TaskPiplineBuilder()
        {
            _components = new List<Func<RpcRequestDelegate, RpcRequestDelegate>>();
        }

        /// <summary>
        /// 使用管道中间件
        /// </summary>
        public TaskPiplineBuilder Use(Func<RpcContext, RpcRequestDelegate, Task> middleware)
        {
            _components.Add(next => context => middleware(context, next));
            return this;
        }
        /// <summary>
        /// 构建处理管道
        /// </summary>
        public RpcRequestDelegate Build(RpcRequestDelegate _complete)
        {
            var invoke = _complete;
            foreach (var component in _components.Reverse())
                invoke = component(invoke);
            return invoke;
        }
    }
}
