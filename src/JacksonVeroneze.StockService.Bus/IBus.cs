using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;

namespace JacksonVeroneze.StockService.Bus
{
    public interface IBus
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task<bool> SendCommand<T>(T comando) where T : Command;

        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;
    }
}
