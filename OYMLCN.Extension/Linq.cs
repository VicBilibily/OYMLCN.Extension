using System.Linq;

namespace OYMLCN
{
    /// <summary>
    /// 排序方式
    /// </summary>
    public enum LinqOrderType : byte
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 顺序
        /// </summary>
        Asc,
        /// <summary>
        /// 倒序
        /// </summary>
        Desc
    }
    /// <summary>
    /// LinqExtensions
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// 获取分页页数据
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IQueryable<TSource> TakePage<TSource>(this IQueryable<TSource> source, int page, int limit = 10)
            => source.Skip((page - 1) * limit).Take(limit);

        /// <summary>
        /// 数据排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static IQueryable<TSource> OrderByField<TSource>(this IQueryable<TSource> source, string field, LinqOrderType order)
        {
            if (field.IsNotNull())
                switch (order)
                {
                    case LinqOrderType.Asc:
                        return source.OrderBy(d => d.GetType().GetProperty(field).GetValue(d, null));
                    case LinqOrderType.Desc:
                        return source.OrderByDescending(d => d.GetType().GetProperty(field).GetValue(d, null));
                }
            return source;
        }


    }
}
