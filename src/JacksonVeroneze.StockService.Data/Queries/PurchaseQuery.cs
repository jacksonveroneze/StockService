using System;
using System.Linq.Expressions;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Data.Queries
{
    public static class PurchaseQuery
    {
        public static Expression<Func<Purchase, bool>> GetQuery(PurchaseFilter filter)
        {
            Expression<Func<Purchase, bool>> expression = order => true;

            if (!string.IsNullOrEmpty(filter.Description))
                expression = expression.And(x => x.Description.Contains(filter.Description));

            if (filter.State.HasValue)
                expression = expression.And(x => x.State == (PurchaseState)Enum.Parse(typeof(PurchaseState),
                    filter.State.ToString()));

            if (filter.DateInitial.HasValue)
                expression = expression.And(x => x.Date >= filter.DateInitial);

            if (filter.DateEnd.HasValue)
                expression = expression.And(x => x.Date <= filter.DateEnd);

            return expression;
        }
    }
}
