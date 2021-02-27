using System;
using System.Linq.Expressions;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Queries;

namespace JacksonVeroneze.StockService.Domain.Filters
{
    public class PurchaseFilter : BaseFilter<Purchase>
    {
        public string Description { get; set; }

        public int? State { get; set; }

        public DateTime? DateInitial { get; set; }

        public DateTime? DateEnd { get; set; }

        public override Expression<Func<Purchase, bool>> ToQuery()
            => PurchaseQuery.GetQuery(this);
    }
}
