using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        private readonly IBus _bus;

        public PurchaseService(IPurchaseRepository repository, IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        public async Task AddItemAsync(Purchase purchase, PurchaseItem item)
        {
            purchase.AddItem(item);

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new PurchaseItemAdded(item.Id));
        }

        public async Task UpdateItemAsync(Purchase purchase, PurchaseItem item)
        {
            purchase.UpdateItem(item);

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new PurchaseItemUpdated(item.Id));
        }

        public async Task RemoveItemAsync(Purchase purchase, PurchaseItem item)
        {
            purchase.RemoveItem(item);

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new PurchaseItemRemoved(item.Id));
        }

        public async Task CloseAsync(Purchase purchase)
        {
            purchase.Close();

            _repository.Update(purchase);

            purchase.AddEvent(new PurchaseClosedEvent(purchase.Id));

            await _repository.UnitOfWork.CommitAsync();
        }
    }
}
