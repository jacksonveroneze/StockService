using System;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Filters
{
    public class MovementFilter : BaseFilter<Movement>
    {
        public Guid? productId { get; set; }
    }
}
