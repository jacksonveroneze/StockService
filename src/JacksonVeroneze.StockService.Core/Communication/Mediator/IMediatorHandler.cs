using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.Notifications;

namespace JacksonVeroneze.StockService.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task<bool> SendCommand<T>(T comando) where T : Command;

        Task PublishNotification<T>(T notification) where T : DomainNotification;

        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;
    }
}
