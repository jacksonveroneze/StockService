using System;
using System.Linq.Expressions;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Queries
{
    public static class ProductQuery
    {
        public static Expression<Func<Product, bool>> GetQuery(ProductFilter filter)
        {
            Expression<Func<Product, bool>> expression = query => true;

            if (!string.IsNullOrEmpty(filter.Description))
                expression = expression.And(x => x.Description.Contains(filter.Description));

            if (filter.IsActive.HasValue)
                expression = expression.And(x => x.IsActive == filter.IsActive);

            return expression;
        }
    }
}
