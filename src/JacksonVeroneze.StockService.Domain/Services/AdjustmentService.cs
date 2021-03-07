using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    /// <summary>
    /// Method responsible for service.
    /// </summary>
    public class AdjustmentService : IAdjustmentService
    {
        private readonly IAdjustmentRepository _repository;
        private readonly IBus _bus;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="bus"></param>
        public AdjustmentService(IAdjustmentRepository repository, IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        /// <summary>
        /// Method responsible for add item.
        /// </summary>
        /// <param name="adjustment"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddItemAsync(Adjustment adjustment, AdjustmentItem item)
        {
            adjustment.AddItem(item);

            _repository.Update(adjustment);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new AdjustmentItemAdded(item.Id));
        }

        /// <summary>
        /// Method responsible for update item.
        /// </summary>
        /// <param name="adjustment"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateItemAsync(Adjustment adjustment, AdjustmentItem item)
        {
            adjustment.UpdateItem(item);

            _repository.Update(adjustment);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new AdjustmentItemUpdated(item.Id));
        }

        /// <summary>
        /// Method responsible for remove item.
        /// </summary>
        /// <param name="adjustment"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task RemoveItemAsync(Adjustment adjustment, AdjustmentItem item)
        {
            adjustment.RemoveItem(item);

            _repository.Update(adjustment);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new AdjustmentItemRemoved(item.Id));
        }

        /// <summary>
        /// Method responsible for close.
        /// </summary>
        /// <param name="adjustment"></param>
        /// <returns></returns>
        public async Task CloseAsync(Adjustment adjustment)
        {
            adjustment.Close();

            _repository.Update(adjustment);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new AdjustmentClosedEvent(adjustment.Id));
        }
    }
}
