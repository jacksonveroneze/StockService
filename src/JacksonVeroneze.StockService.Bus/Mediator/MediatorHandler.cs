using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.Notifications;
using MediatR;

namespace JacksonVeroneze.StockService.Bus.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<bool> SendCommand<T>(T comando) where T : Command
            => await _mediator.Send(comando);

        public async Task PublishEvent<T>(T evento) where T : Event
            => await _mediator.Publish(evento);

        public async Task PublishNotification<T>(T notification) where T : DomainNotification
            => await _mediator.Publish(notification);

        public async Task PublishDomainEvent<T>(T notification) where T : DomainEvent
            => await _mediator.Publish(notification);
    }
}
