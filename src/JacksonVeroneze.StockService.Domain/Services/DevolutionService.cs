using System.Threading.Tasks;
using JacksonVeroneze.StockService.Infra.Bus;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Devolution;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    /// <summary>
    /// Method responsible for service.
    /// </summary>
    public class DevolutionService : IDevolutionService
    {
        private readonly IDevolutionRepository _repository;
        private readonly IBus _bus;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="bus"></param>
        public DevolutionService(IDevolutionRepository repository, IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        /// <summary>
        /// Method responsible for add.
        /// </summary>
        /// <param name="devolution"></param>
        /// <returns></returns>
        public async Task AddAsync(Devolution devolution)
        {
            await _repository.AddAsync(devolution);

            await _repository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Method responsible for add item.
        /// </summary>
        /// <param name="devolution"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddItemAsync(Devolution devolution, DevolutionItem item)
        {
            devolution.AddItem(item);

            _repository.Update(devolution);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new DevolutionItemAdded(item.Id));
        }

        /// <summary>
        /// Method responsible for update item.
        /// </summary>
        /// <param name="devolution"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateItemAsync(Devolution devolution, DevolutionItem item)
        {
            devolution.UpdateItem(item);

            _repository.Update(devolution);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new DevolutionItemUpdated(item.Id));
        }

        /// <summary>
        /// Method responsible for remove item.
        /// </summary>
        /// <param name="devolution"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task RemoveItemAsync(Devolution devolution, DevolutionItem item)
        {
            devolution.RemoveItem(item);

            _repository.Update(devolution);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new DevolutionItemRemoved(item.Id));
        }

        /// <summary>
        /// Method responsible for close.
        /// </summary>
        /// <param name="devolution"></param>
        /// <returns></returns>
        public async Task CloseAsync(Devolution devolution)
        {
            devolution.Close();

            _repository.Update(devolution);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new DevolutionClosedEvent(devolution.Id));
        }
    }
}
