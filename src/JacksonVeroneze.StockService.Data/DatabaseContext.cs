using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JacksonVeroneze.StockService.Data
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        private readonly IBus _bus;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IBus bus)
            : base(options)
            => _bus = bus;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            modelBuilder.Ignore<Event>();

            modelBuilder.Entity<Adjustment>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<AdjustmentItem>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Output>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<OutputItem>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Purchase>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<PurchaseItem>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Movement>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<MovementItem>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<Product>().HasQueryFilter(x => x.DeletedAt == null);
        }

        public async Task<bool> CommitAsync()
        {
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
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
}
