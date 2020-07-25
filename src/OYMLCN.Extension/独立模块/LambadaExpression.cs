using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OYMLCN
{
    /// <summary>
    /// 构建查询条件（由于本扩展与第三方ORM扩展冲突，v3.x开始采用封闭式封装，表达式接在由 <see cref="LambadaExpression{T}.Result"/> 输出）
    /// </summary>
    public class LambadaExpression
    {
        /// <summary>
        /// 创建条件（由于本扩展与第三方ORM扩展冲突，v3.x开始采用封闭式封装，表达式接在由 <see cref="LambadaExpression{T}.Result"/> 输出）
        /// </summary>
        public static LambadaExpression<Target> Create<Target>(Expression<Func<Target, bool>> exp) => new LambadaExpression<Target>(exp);
        /// <summary>
        /// 起始条件为True（由于本扩展与第三方ORM扩展冲突，v3.x开始采用封闭式封装，表达式接在由 <see cref="LambadaExpression{T}.Result"/> 输出）
        /// </summary>
        public static LambadaExpression<Target> CreateTrue<Target>() => Create<Target>(v => true);
        /// <summary>
        /// 起始条件为False（由于本扩展与第三方ORM扩展冲突，v3.x开始采用封闭式封装，表达式接在由 <see cref="LambadaExpression{T}.Result"/> 输出）
        /// </summary>
        public static LambadaExpression<Target> CreateFalse<Target>() => Create<Target>(v => false);
    }
    /// <summary>
    /// 构建查询条件（由于本扩展与第三方ORM扩展冲突，v3.x开始采用封闭式封装，表达式接在由 <see cref="LambadaExpression{T}.Result"/> 输出）
    /// </summary>
    public class LambadaExpression<T>
    {
        /// <summary>
        /// 构建查询条件（由于本扩展与第三方ORM扩展冲突，v3.x开始采用封闭式封装，表达式接在由 <see cref="LambadaExpression{T}.Result"/> 输出）
        /// </summary>
        public LambadaExpression(Expression<Func<T, bool>> exp)
        {
            this.Result = exp;
        }
        /// <summary>
        /// 条件表达式输出
        /// </summary>
        public Expression<Func<T, bool>> Result { get; private set; }

        /// <summary>
        /// 使用 and 拼接两个 lambda 表达式
        /// </summary>
        public LambadaExpression<T> And(Expression<Func<T, bool>> exp) => And(true, exp);
        /// <summary>
        /// 使用 and 拼接两个 lambda 表达式
        /// </summary>
        /// <param name="condition">true 时生效</param>
        /// <param name="exp"></param>
        public LambadaExpression<T> And(bool condition, Expression<Func<T, bool>> exp)
        {
            if (condition == false) return this;
            Result = Compose(Result, exp, Expression.AndAlso);
            return this;
        }
        /// <summary>
        /// 使用 or 拼接两个 lambda 表达式
        /// </summary>
        public LambadaExpression<T> Or(Expression<Func<T, bool>> exp) => Or(true, exp);
        /// <summary>
        /// 使用 or 拼接两个 lambda 表达式
        /// </summary>
        /// <param name="condition">true 时生效</param>
        /// <param name="exp"></param>
        public LambadaExpression<T> Or(bool condition, Expression<Func<T, bool>> exp)
        {
            if (condition == false) return this;
            Result = Compose(Result, exp, Expression.OrElse);
            return this;
        }
        /// <summary>
        /// 将 lambda 表达式取反
        /// </summary>
        /// <param name="condition">true 时生效</param>
        public LambadaExpression<T> Not(bool condition = true)
        {
            if (condition == false) return this;
            Result = Expression.Lambda<Func<T, bool>>(Expression.Not(Result.Body), Result.Parameters);
            return this;
        }

        #region 内部合并操作方法
        Expression<Target> Compose<Target>(Expression<Target> first, Expression<Target> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<Target>(merge(first.Body, secondBody), first.Parameters);
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
        #endregion
    }
}
