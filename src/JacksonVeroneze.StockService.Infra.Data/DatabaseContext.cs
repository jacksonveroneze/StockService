using System;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Infra.Bus;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Infra.Data.Util;
using JacksonVeroneze.StockService.Domain.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JacksonVeroneze.StockService.Infra.Data
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        private readonly IBus _bus;
        private readonly IUser _user;
        private readonly Guid _teantId = Guid.Parse("09163A85-D55E-4E2F-B7E2-DD191FE22A6E");

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IBus bus, IUser user)
            : base(options)
        {
            _bus = bus;
            _user = user;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            modelBuilder.Ignore<Event>();

            modelBuilder.HasDefaultSchema("stock");

            Guid tentantId = _teantId;

            modelBuilder.AddFilter<Adjustment>(tentantId);
            modelBuilder.AddFilter<AdjustmentItem>(tentantId);
            modelBuilder.AddFilter<Output>(tentantId);
            modelBuilder.AddFilter<OutputItem>(tentantId);
            modelBuilder.AddFilter<Purchase>(tentantId);
            modelBuilder.AddFilter<PurchaseItem>(tentantId);
            modelBuilder.AddFilter<Movement>(tentantId);
            modelBuilder.AddFilter<MovementItem>(tentantId);
            modelBuilder.AddFilter<Product>(tentantId);
        }

        public async Task<bool> CommitAsync()
        {
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added &&
                    entry.Members.Any(x => x.Metadata.Name.Equals(nameof(Entity.TenantId))))
                    entry.Property(nameof(Entity.TenantId)).CurrentValue = _teantId;

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(Entity.UpdatedAt)).CurrentValue = DateTime.Now;
                    entry.Property(nameof(Entity.Version)).CurrentValue =
                        (int)entry.Property(nameof(Entity.Version)).CurrentValue + 1;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;

                    if (entry.Members.Any(x => x.Metadata.Name.Equals(nameof(Entity.DeletedAt))))
                        entry.Property(nameof(Entity.DeletedAt)).CurrentValue = DateTime.Now;
                }
            }

            bool isSuccess = await base.SaveChangesAsync() > 0;

            if (isSuccess)
                await _bus.PublishEvents(this);

            return isSuccess;
        }
    }

    public static class AddGlobalFilterExtension
    {
        public static void AddFilter<T>(this ModelBuilder modelBuilder, Guid tenantId) where T : Entity
        {
            modelBuilder.Entity<T>()
                .HasQueryFilter(x => x.DeletedAt == null && x.TenantId == tenantId);
        }
    }
}
