using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Adjustment;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class AdjustmentService : IAdjustmentService
    {
        private readonly IAdjustmentRepository _repository;
        private readonly IBusHandler _busHandler;

        public AdjustmentService(IAdjustmentRepository repository, IBusHandler busHandler)
        {
            _repository = repository;
            _busHandler = busHandler;
        }

        public async Task AddItemAsync(Adjustment adjustment, AdjustmentItem item)
        {
            adjustment.AddItem(item);

            _repository.Update(adjustment);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new AdjustmentItemAdded(item.Id));
        }

        public async Task UpdateItemAsync(Adjustment adjustment, AdjustmentItem item)
        {
            adjustment.UpdateItem(item);

            _repository.Update(adjustment);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new AdjustmentItemUpdated(item.Id));
        }

        public async Task RemoveItemAsync(Adjustment adjustment, AdjustmentItem item)
        {
            adjustment.RemoveItem(item);

            _repository.Remove(adjustment);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new AdjustmentItemRemoved(item.Id));
        }

        public async Task CloseAsync(Adjustment adjustment)
        {
            adjustment.Close();

            _repository.Update(adjustment);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new AdjustmentClosed(adjustment.Id));
        }
    }
}
