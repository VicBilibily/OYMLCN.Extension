using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OYMLCN.RPC.Core.RpcBuilder
{
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
