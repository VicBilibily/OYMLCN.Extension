using System;

namespace OYMLCN.RPC.Core
{

    /// <summary>
    /// 远程调用返回数据使用缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RpcCacheAttribute : Attribute
    {
        /// <summary>
        /// 远程调用返回数据使用缓存（默认缓存10s）
        /// </summary>
        /// <param name="cacheTime"> 缓存时间（默认10s） </param>
        public RpcCacheAttribute(int cacheTime = 10)
        {
            CacheTime = cacheTime;
        }
        /// <summary>
        /// 缓存时间（单位:秒）
        /// </summary>
        public int CacheTime { get; private set; } = 10;
        /// <summary>
        /// 是否进行参数缓存（默认开启）
        /// </summary>
        public bool CacheParameters { get; set; } = true;
        /// <summary>
        /// 使用用户登录Token进行区分缓存（默认关闭）
        /// </summary>
        public bool CacheToken { get; set; } = false;

        /// <summary>
        /// 缓存上下文参数（多个参数则用英文逗号[,]区分）
        /// </summary>
        public string CacheSession { get; set; }

        /// <summary>
        /// 不缓存输出结果（目标方法有效设置优先）
        /// </summary>
        public bool NoCache { get; set; }
    }
}
