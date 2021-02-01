using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.Notifications;

namespace JacksonVeroneze.StockService.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;

        Task<bool> EnviarComando<T>(T comando) where T : Command;

        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;

        Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent;
    }
}
