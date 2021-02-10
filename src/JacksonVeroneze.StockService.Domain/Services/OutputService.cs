using JacksonVeroneze.StockService.Bus.Mediator;
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
    }
}
