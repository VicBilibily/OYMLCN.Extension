using Microsoft.Extensions.DependencyInjection;
using OYMLCN.RPC.Core.RpcBuilder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 辅助方法
    /// </summary>
    public static class Utils
    {
        private static readonly ConcurrentDictionary<string, IEnumerable<PropertyInfo>> _filterFromServices = new ConcurrentDictionary<string, IEnumerable<PropertyInfo>>();
        /// <summary>
        /// 获取指定的过滤器实例
        /// </summary>
        public static RpcFilterAttribute GetRpcFilterInstance(IServiceProvider serviceProvider, Type filterType)
            => ActivatorUtilities.CreateInstance(serviceProvider, filterType) as RpcFilterAttribute;
        /// <summary>
        /// 获取指定的过滤器实例集合
        /// </summary>
        public static IEnumerable<RpcFilterAttribute> GetRpcFilterInstances(IServiceProvider serviceProvider, IEnumerable<Type> filterTypes)
        {
            foreach (var filterType in filterTypes)
                yield return GetRpcFilterInstance(serviceProvider, filterType);
        }

        /// <summary>
        /// 服务注入对象
        /// </summary>
        public static void PropertieFromServicesInject<T>(IServiceProvider serviceProvider, T rpcFilterAttribute)
        {
            var properties = _filterFromServices.GetOrAdd($"{rpcFilterAttribute.GetType().FullName}", key => rpcFilterAttribute.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(i => i.GetCustomAttribute<FromServicesAttribute>() != null));
            if (properties.Any())
                foreach (var propertyInfo in properties)
                    propertyInfo.SetValue(rpcFilterAttribute, serviceProvider.GetService(propertyInfo.PropertyType));
        }
        /// <summary>
        /// 服务注入对象
        /// </summary>
        public static void PropertiesFromServicesInject(IServiceProvider serviceProvider, IEnumerable<RpcFilterAttribute> rpcFilterAttributes)
        {
            foreach (var fitler in rpcFilterAttributes)
                PropertieFromServicesInject(serviceProvider, fitler);
        }
    }
}
