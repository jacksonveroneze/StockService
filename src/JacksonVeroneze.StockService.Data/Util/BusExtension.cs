using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.Messages;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JacksonVeroneze.StockService.Data.Util
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IBus bus, DatabaseContext dbContext)
        {
            IList<EntityEntry<Entity>> domainEntities = dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any())
                .ToList();

            if (domainEntities.Any() is false) return;

            IList<Event> domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            IEnumerable<Task> tasks = domainEvents
                .Select(async domainEvent => { await bus.PublishEvent(domainEvent); });

            await Task.WhenAll(tasks);
        }
    }
}
