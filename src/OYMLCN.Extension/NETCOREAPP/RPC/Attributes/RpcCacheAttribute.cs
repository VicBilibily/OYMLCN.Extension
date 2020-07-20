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
            this.CacheTime = cacheTime;
        }
        /// <summary>
        /// 缓存时间（单位:秒）
        /// </summary>
        public int CacheTime { get; private set; }
        /// <summary>
        /// 是否进行参数缓存
        /// </summary>
        public bool CacheParameters { get; set; }

        /// <summary>
        /// 缓存上下文参数（多个参数则用英文逗号[,]区分）
        /// </summary>
        public string CacheSession { get; set; }

        /// <summary>
        /// 不缓存输出结果（仅对目标方法有效）
        /// </summary>
        public bool NoCache { get; set; }
    }
}
