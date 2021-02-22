using System;
using System.Linq.Expressions;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Data.Queries
{
    public static class PurchaseQuery
    {
        public static Expression<Func<Purchase, bool>> GetQuery(PurchaseFilter filter)
        {
            Expression<Func<Purchase, bool>> expression = order => true;

            if (!string.IsNullOrEmpty(filter.Description))
            {
                Expression<Func<Purchase, bool>> expressionDescription =
                    x => x.Description.Contains(filter.Description);

                expression = expression.And(expressionDescription);
            }

            return expression;
        }
    }
}
