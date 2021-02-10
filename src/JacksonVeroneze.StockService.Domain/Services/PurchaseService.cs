using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        private readonly IMediatorHandler _mediatorHandler;

        public PurchaseService(IPurchaseRepository repository, IMediatorHandler mediatorHandler)
        {
            _repository = repository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task AddItem(Purchase purchase, PurchaseItem item)
        {
            purchase.AddItem(item);

            await _repository.AddAsync(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _mediatorHandler.PublishDomainEvent(new PurchaseItemAdded(item.Id));
        }

        public async Task RemoveItem(Purchase purchase, PurchaseItem item)
        {
            purchase.RemoveItem(item);

            _repository.Remove(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _mediatorHandler.PublishDomainEvent(new PurchaseItemRemoved(item.Id));
        }

        public async Task Close(Purchase purchase)
        {
            purchase.Close();

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                foreach (PurchaseItem purchaseItem in purchase.Items)
                    await _mediatorHandler.PublishDomainEvent(new PurchaseClosed(purchase.Id, purchaseItem.Id));
        }
    }
}
