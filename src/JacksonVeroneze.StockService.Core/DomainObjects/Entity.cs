using System;

namespace JacksonVeroneze.StockService.Core.DomainObjects
{
    public abstract class Entity : EntityId
    {
        public DateTime CreatedAt { get; } = DateTime.Now;

        public DateTime? UpdatedAt { get; private set; }

        public DateTime? DeletedAt { get; private set; }

        public int Version { get; } = 1;

        public Guid TenantId { get; private set; }

        public Entity ShallowCopy()
            => (Entity)MemberwiseClone();

        public override string ToString()
            => $"{GetType().Name}: Id: {Id}, CreatedAt: {CreatedAt}, " +
               $"UpdatedAt: {UpdatedAt}, DeletedAt: {DeletedAt}, Version: {Version}, TenantId: {TenantId}";
    }
}
