using System;
using System.Linq.Expressions;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Queries
{
    public static class OutputQuery
    {
        public static Expression<Func<Output, bool>> GetQuery(OutputFilter filter)
        {
            Expression<Func<Output, bool>> expression = order => true;

            if (!string.IsNullOrEmpty(filter.Description))
                expression = expression.And(x => x.Description.Contains(filter.Description));

            if (filter.State.HasValue)
                expression = expression.And(x => x.State == (OutputState)Enum.Parse(typeof(OutputState),
                    filter.State.ToString()));

            if (filter.DateInitial.HasValue)
                expression = expression.And(x => x.Date >= filter.DateInitial);

            if (filter.DateEnd.HasValue)
                expression = expression.And(x => x.Date <= filter.DateEnd);

            return expression;
        }
    }
}
