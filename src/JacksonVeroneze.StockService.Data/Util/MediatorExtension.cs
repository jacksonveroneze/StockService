using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.DomainObjects;
using MediatR;

namespace JacksonVeroneze.StockService.Data.Util
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IMediator mediator, DatabaseContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => { await mediator.Publish(domainEvent); });

            await Task.WhenAll(tasks);
        }
    }
}
