using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JacksonVeroneze.StockService.Domain.Util
{
    public static class PredicateExpressionExtension
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            ParameterExpression parameter = a.Parameters[0];
            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[b.Parameters[0]] = parameter;
            BinaryExpression body = Expression.AndAlso(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            ParameterExpression parameter = a.Parameters[0];
            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[b.Parameters[0]] = parameter;
            BinaryExpression body = Expression.Or(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }

    internal class SubstExpressionVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> subst = new();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (subst.TryGetValue(node, out Expression newValue))
                return newValue;

            return node;
        }
    }
}
