using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Messages;
using JacksonVeroneze.StockService.Core.Messages.CommonMessages.DomainEvents;
using MassTransit;

namespace JacksonVeroneze.StockService.Infra.Bus.MassTransit
{
    public class BusMassTransit : IBusExternal
    {
        private readonly IBusControl _bus;

        public BusMassTransit(IBusControl massTransit)
            => _bus = massTransit;

        public async Task PublishEvent<T>(T evento) where T : Event
            => await _bus.Publish(evento);

        public async Task PublishDomainEvent<T>(T notification) where T : DomainEvent
            => await _bus.Publish(notification);
    }
}
