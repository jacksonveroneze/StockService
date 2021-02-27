using System;
using System.Linq.Expressions;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Queries;

namespace JacksonVeroneze.StockService.Domain.Filters
{
    public class ProductFilter : BaseFilter<Product>
    {
        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public override Expression<Func<Product, bool>> ToQuery()
            => ProductQuery.GetQuery(this);
    }
}
