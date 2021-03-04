using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JacksonVeroneze.StockService.Data
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

            //var claim = _user.GetClaimsIdentity().FirstOrDefault(x => x.Type == ClaimTypes.UserData);

            var tentantId = _teantId;

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
                if (entry.State == EntityState.Added)
                    entry.Property("TenantId").CurrentValue = _teantId;

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                    entry.Property("Version").CurrentValue = (int)entry.Property("Version").CurrentValue + 1;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;

                    entry.Property("DeletedAt").CurrentValue = DateTime.Now;
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
        public static ModelBuilder AddFilter<T>(this ModelBuilder modelBuilder, Guid tenantId) where T : Entity
        {
            modelBuilder.Entity<T>()
                .HasQueryFilter(x => x.DeletedAt == null && x.TenantId == tenantId);

            return modelBuilder;
        }
    }
}
