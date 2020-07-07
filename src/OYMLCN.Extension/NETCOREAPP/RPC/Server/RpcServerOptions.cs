using Microsoft.Extensions.DependencyInjection;
using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OYMLCN.RPC.Server
{
    /// <summary>
    /// 过程调用配置类
    /// </summary>
    public class RpcServerOptions
    {
        /// <summary>
        /// 默认过程调用地址（默认为\"/__rpc__\"，null或空所有请求都会尝试进入调用过程处理）
        /// </summary>
        public string RpcUrl { get; private set; } = "/__rpc__";
        /// <summary>
        /// 默认调用的接口
        /// </summary>
        public string DefaultInterface { get; private set; }
        private readonly IDictionary<string, Type[]> _interface = new Dictionary<string, Type[]>();
        private readonly IDictionary<string, Type> _types = new Dictionary<string, Type>();
        private readonly IList<Type> _filterTypes = new List<Type>();
        private readonly IServiceCollection _services;

        /// <summary>
        /// 过程调用配置类
        /// </summary>
        public RpcServerOptions(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// 设置过程调用处理管道入口
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public RpcServerOptions SetRpcUrl(string path)
        {
            this.RpcUrl = path;
            return this;
        }
        /// <summary>
        /// 设置找不到方法时查找的默认接口
        /// </summary>
        public RpcServerOptions SetDefaultInterface(string typeName)
        {
            if (_interface.ContainsKey(typeName))
                this.DefaultInterface = typeName;
            else
                throw new ArgumentException("设置的默认接口未完成注册", typeName);
            return this;
        }
        /// <summary>
        /// 设置找不到方法时查找的默认接口
        /// </summary>
        public RpcServerOptions SetDefaultInterface<T>()
        {
            return SetDefaultInterface(typeof(T).FullName);
        }

        /// <summary>
        /// 添加过程调用过滤器
        /// </summary>
        public RpcServerOptions AddFilter<RpcFilterAttribute>()
        {
            _filterTypes.Add(typeof(RpcFilterAttribute));
            return this;
        }

        /// <summary>
        /// 添加过程调用接口
        /// </summary>
        public RpcServerOptions AddInterface(string interfaceName)
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            Type @interface = null;
            foreach (var item in ass)
            {
                @interface = item.ExportedTypes.Where(v => v.FullName == interfaceName).FirstOrDefault();
                if (@interface != null) break;
            }
            if (@interface == null) throw new ArgumentException("未找到指定的接口类型", nameof(interfaceName));

            var types = new List<Type>();
            foreach (var item in ass)
                types.AddRange(item.ExportedTypes.Where(v => v.IsClass && v.GetInterfaces().Contains(@interface)));
            _interface.TryAdd(interfaceName, types.ToArray());
            types.ForEach(type => _types.TryAdd(type.FullName, type));
            return this;
        }
        /// <summary>
        /// 添加过程调用接口
        /// </summary>
        public RpcServerOptions AddInterface<T>() where T : class
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            Type @interface = typeof(T);
            var types = new List<Type>();
            foreach (var item in ass)
                types.AddRange(item.ExportedTypes.Where(v => v.IsClass && v.GetInterfaces().Contains(@interface)));
            _interface.TryAdd(@interface.FullName, types.ToArray());
            types.ForEach(type => _types.TryAdd(type.FullName, type));
            return this;
        }
        /// <summary>
        /// 添加过程调用接口
        /// </summary>
        public RpcServerOptions AddNameSpace(string nameSpace)
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            var interfaces = new List<Type>();
            foreach (var item in ass)
                interfaces.AddRange(item.ExportedTypes.Where(v => v.IsInterface && v.Namespace == nameSpace));
            foreach (var @interface in interfaces)
            {
                var types = new List<Type>();
                foreach (var item in ass)
                    types.AddRange(item.ExportedTypes.Where(v => v.IsClass && v.GetInterfaces().Contains(@interface)));
                _interface.TryAdd(@interface.FullName, types.ToArray());
                types.ForEach(type => _types.TryAdd(type.FullName, type));
            }
            return this;
        }

        /// <summary>
        /// 获取接口及实现接口的类型
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, Type[]> GetInterfaceTypes()
            => _interface.ToDictionary(i => i.Key, i => i.Value.ToArray());
        /// <summary>
        /// 获取已注册的类型
        /// </summary>
        public IDictionary<string, Type> GetTypes()
            => _types.ToDictionary(i => i.Key, i => i.Value);
        /// <summary>
        /// 获取已注册的过滤器
        /// </summary>
        public IEnumerable<Type> GetFilterTypes() => _filterTypes.ToArray();
    }

}
