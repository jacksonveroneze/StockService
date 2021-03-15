using System;
using System.Linq.Expressions;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Queries;

namespace JacksonVeroneze.StockService.Domain.Filters
{
    public class MovementFilter : BaseFilter<Movement>
    {
        public Guid? productId { get; set; }

        public override Expression<Func<Movement, bool>> ToQuery()
            => MovementQuery.GetQuery(this);
    }
}
