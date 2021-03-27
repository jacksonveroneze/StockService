using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Infra.Bus
{
    public interface IBusExternal
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;
    }
}
