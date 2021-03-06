using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OYMLCN
{

    /// <summary>    
    /// 构建查询条件    
    /// </summary>    
    public static class PredicateBuilder
    {
        /// <summary>
        /// 起始条件为True
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() => param => true;

        /// <summary>
        /// 起始条件为False
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() => param => false;

        /// <summary>
        /// 创建条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) => predicate;
        /// <summary>
        /// 组合And条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
            => first.Compose(second, Expression.AndAlso);

        /// <summary>
        /// 组合Or条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
            => first.Compose(second, Expression.OrElse);

        /// <summary>
        /// 组合Not条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
            => Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);

        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)    
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first    
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression    
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
                => this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
                => new ParameterRebinder(map).Visit(exp);

            protected override Expression VisitParameter(ParameterExpression p)
            {
                if (map.TryGetValue(p, out ParameterExpression replacement))
                    p = replacement;
                return base.VisitParameter(p);
            }
        }
    }
}
