using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN
{
    /// <summary>
    /// 定长队列（线程安全的先进先出集合）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentLimitedQueue<T> : ConcurrentQueue<T>
    {
        /// <summary>
        /// 队列长度
        /// </summary>
        protected int Limit { get; set; }
        /// <summary>
        /// 定长队列
        /// </summary>
        /// <param name="limit">队列长度</param>
        public ConcurrentLimitedQueue(int limit)
        {
            Limit = limit;
        }
        /// <summary>
        /// 定长队列
        /// </summary>
        /// <param name="list">已有队列</param>
        public ConcurrentLimitedQueue(IEnumerable<T> list) : base(list)
        {
            Limit = list.Count();
        }
        /// <summary>
        /// 插入队列
        /// </summary>
        /// <param name="item"></param>
        public new void Enqueue(T item)
        {
            if (Count >= Limit)
                TryDequeue(out var _);
            base.Enqueue(item);
        }
    }
}
