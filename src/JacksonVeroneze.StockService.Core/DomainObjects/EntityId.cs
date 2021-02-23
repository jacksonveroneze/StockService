using System;

namespace JacksonVeroneze.StockService.Core.DomainObjects
{
    public class EntityId
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
