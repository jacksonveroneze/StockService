using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Infra.Bus;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Devolution;
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Services
{
    /// <summary>
    /// Method responsible for service.
    /// </summary>
    public class OutputService : IOutputService
    {
        private readonly IOutputRepository _repository;
        private readonly IBus _bus;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="bus"></param>
        public OutputService(IOutputRepository repository, IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        /// <summary>
        /// Method responsible for item.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task AddAsync(Output output)
        {
            await _repository.AddAsync(output);

            await _repository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Method responsible for add item.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddItemAsync(Output output, OutputItem item)
        {
            output.AddItem(item);

            _repository.Update(output);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new OutputItemAdded(item.Id));
        }

        /// <summary>
        /// Method responsible for update item.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateItemAsync(Output output, OutputItem item)
        {
            output.UpdateItem(item);

            _repository.Update(output);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new OutputItemUpdated(item.Id));
        }

        /// <summary>
        /// Method responsible for remove item.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task RemoveItemAsync(Output output, OutputItem item)
        {
            output.RemoveItem(item);

            _repository.Update(output);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new OutputItemRemoved(item.Id));
        }

        /// <summary>
        /// Method responsible for close.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task CloseAsync(Output output)
        {
            output.Close();

            _repository.Update(output);

            if (await _repository.UnitOfWork.CommitAsync())
                await _bus.PublishDomainEvent(new OutputClosedEvent(output.Id));
        }

        /// <summary>
        /// Method responsible for undo item.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UndoItemAsync(Output output, OutputItem item)
        {
            output.Open();

            output.RemoveItem(item);

            output.Close();

            _repository.Remove(output);

            // MovementItem movementItem = item.MovementItems.FirstOrDefault();
            //
            // if (movementItem is null)
            //     throw ExceptionsFactory.FactoryDomainException(Messages.MovementItemNotFound);

            if (await _repository.UnitOfWork.CommitAsync())
            {
                // await _bus.PublishDomainEvent(new OutputItemUndoEvent(movementItem.Movement.Id, movementItem.Id,
                //     item.Amount));

                await _bus.PublishDomainEvent(
                    new OutputItemUndoDevolutionEvent(item.Id, output.Description, item.Amount, item.Product.Id));
            }
        }
    }
}
