using OYMLCN.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OYMLCN.Extensions
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
        {
            int skip = (page - 1) * limit;
            return source.Skip(skip < 0 ? 0 : skip).Take(limit);
        }

        /// <summary>
        /// 获取分页页数据（自动获取有效数据）
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IQueryable<TSource> TakePageAuto<TSource>(this IQueryable<TSource> source, int page, int limit = 10)
            => TakePageAuto(source, page, out PaginationHelper pagination, limit);

        /// <summary>
        /// 获取分页页数据（自动获取有效数据）
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pagination">分页结果</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IQueryable<TSource> TakePageAuto<TSource>(this IQueryable<TSource> source, int page, out PaginationHelper pagination, int limit = 10)
        {
            pagination = new PaginationHelper(source.Count(), limit);
            pagination.GetValidPage(page);
            return source.TakePage(pagination.Page, pagination.Limit);
        }

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

        /// <summary>
        /// 与连接
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="left">左条件</param>
        /// <param name="right">右条件</param>
        /// <returns>新表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
            => CombineLambdas(left, right, ExpressionType.AndAlso);
        /// <summary>
        /// 或连接
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="left">左条件</param>
        /// <param name="right">右条件</param>
        /// <returns>新表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
            => CombineLambdas(left, right, ExpressionType.OrElse);
        
        private static Expression<Func<T, bool>> CombineLambdas<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, ExpressionType expressionType)
        {
            if (IsExpressionBodyConstant(left))
                return right;
            var visitor = new SubstituteParameterVisitor
            {
                Sub =
                {
                    [right.Parameters[0]] = left.Parameters[0]
                }
            };
            Expression body = Expression.MakeBinary(expressionType, left.Body, visitor.Visit(right.Body));
            return Expression.Lambda<Func<T, bool>>(body, left.Parameters[0]);
        }
        private static bool IsExpressionBodyConstant<T>(Expression<Func<T, bool>> left)
            => left.Body.NodeType == ExpressionType.Constant;
        internal class SubstituteParameterVisitor : ExpressionVisitor
        {
            public Dictionary<Expression, Expression> Sub = new Dictionary<Expression, Expression>();
            protected override Expression VisitParameter(ParameterExpression node)
                => Sub.TryGetValue(node, out var newValue) ? newValue : node;
        }

    }
}
