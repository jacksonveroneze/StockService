using System;
using System.Linq.Expressions;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Queries
{
    public static class DevolutionQuery
    {
        public static Expression<Func<Devolution, bool>> GetQuery(DevolutionFilter filter)
        {
            Expression<Func<Devolution, bool>> expression = query => true;

            if (!string.IsNullOrEmpty(filter.Description))
                expression = expression.And(x => x.Description.Contains(filter.Description));

            if (filter.State.HasValue)
                expression = expression.And(x => x.State == (DevolutionState)Enum.Parse(typeof(DevolutionState),
                    filter.State.ToString()));

            if (filter.DateInitial.HasValue)
                expression = expression.And(x => x.Date >= filter.DateInitial);

            if (filter.DateEnd.HasValue)
                expression = expression.And(x => x.Date <= filter.DateEnd);

            return expression;
        }
    }
}
