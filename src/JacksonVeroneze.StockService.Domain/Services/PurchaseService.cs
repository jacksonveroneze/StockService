using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        private readonly IBusHandler _busHandler;

        public PurchaseService(IPurchaseRepository repository, IBusHandler busHandler)
        {
            _repository = repository;
            _busHandler = busHandler;
        }

        public async Task AddItem(Purchase purchase, PurchaseItem item)
        {
            purchase.AddItem(item);

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new PurchaseItemAdded(item.Id));
        }

        public async Task UpdateItem(Purchase purchase, PurchaseItem item)
        {
            purchase.UpdateItem(item);

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new PurchaseItemUpdated(item.Id));
        }

        public async Task RemoveItem(Purchase purchase, PurchaseItem item)
        {
            purchase.RemoveItem(item);

            _repository.Remove(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new PurchaseItemRemoved(item.Id));
        }

        public async Task Close(Purchase purchase)
        {
            purchase.Close();

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new PurchaseClosed(purchase.Id));
        }
    }
}
