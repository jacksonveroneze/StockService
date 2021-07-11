using System.Threading.Tasks;
using JacksonVeroneze.StockService.Infra.Bus;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Purchase;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    /// <summary>
    ///
    /// </summary>
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        private readonly IBus _bus;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="bus"></param>
        public PurchaseService(IPurchaseRepository repository, IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        /// <summary>
        /// Method responsible for add item.
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public async Task AddAsync(Purchase purchase)
        {
            await _repository.AddAsync(purchase);

            await _repository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Method responsible for add item.
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddItemAsync(Purchase purchase, PurchaseItem item)
        {
            purchase.AddItem(item);

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new PurchaseItemAdded(item.Id));
        }

        /// <summary>
        /// Method responsible for update item.
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateItemAsync(Purchase purchase, PurchaseItem item)
        {
            purchase.UpdateItem(item);

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new PurchaseItemUpdated(item.Id));
        }

        /// <summary>
        /// Method responsible for remove item.
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task RemoveItemAsync(Purchase purchase, PurchaseItem item)
        {
            purchase.RemoveItem(item);

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new PurchaseItemRemoved(item.Id));
        }

        /// <summary>
        /// Method responsible for close.
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public async Task CloseAsync(Purchase purchase)
        {
            purchase.Close();

            _repository.Update(purchase);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new PurchaseClosedEvent(purchase.Id));
        }
    }
}
