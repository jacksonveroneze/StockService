using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Output;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class OutputService : IOutputService
    {
        private readonly IOutputRepository _repository;
        private readonly IBusHandler _busHandler;

        public OutputService(IOutputRepository repository, IBusHandler busHandler)
        {
            _repository = repository;
            _busHandler = busHandler;
        }

        public async Task AddItemAsync(Output output, OutputItem item)
        {
            output.AddItem(item);

            _repository.Update(output);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new OutputItemAdded(item.Id));
        }

        public async Task UpdateItemAsync(Output output, OutputItem item)
        {
            output.UpdateItem(item);

            _repository.Update(output);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new OutputItemUpdated(item.Id));
        }

        public async Task RemoveItemAsync(Output output, OutputItem item)
        {
            output.RemoveItem(item);

            _repository.Remove(output);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new OutputItemRemoved(item.Id));
        }

        public async Task CloseAsync(Output output)
        {
            output.Close();

            _repository.Update(output);

            if (await _repository.UnitOfWork.CommitAsync())
                await _busHandler.PublishDomainEvent(new OutputClosedEvent(output.Id));
        }
    }
}
