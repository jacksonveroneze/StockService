using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JacksonVeroneze.StockService.Data
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediatorHandler;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IMediator mediatorHandler)
            : base(options)
            => _mediatorHandler = mediatorHandler;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            modelBuilder.Ignore<Event>();

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
                await _mediatorHandler.PublishEvents(this);

            return isSuccess;
        }
    }
}
