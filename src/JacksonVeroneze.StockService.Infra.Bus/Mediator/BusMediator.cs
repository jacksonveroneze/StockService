using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;
using MediatR;

namespace JacksonVeroneze.StockService.Infra.Bus.Mediator
{
    public class BusMediator : IBus
    {
        private readonly IMediator _bus;

        public BusMediator(IMediator mediator)
            => _bus = mediator;

        public async Task PublishEvent<T>(T evento) where T : Event
            => await _bus.Publish(evento);

        public async Task PublishDomainEvent<T>(T notification) where T : DomainEvent
            => await _bus.Publish(notification);
    }
}
