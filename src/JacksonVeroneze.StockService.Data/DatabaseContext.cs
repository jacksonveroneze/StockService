using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Communication.Mediator;
using JacksonVeroneze.StockService.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JacksonVeroneze.StockService.Data
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IMediatorHandler mediatorHandler)
            : base(options)
            => _mediatorHandler = mediatorHandler;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);

        public async Task<bool> Commit()
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

            return await base.SaveChangesAsync() > 0;
        }
    }
}
