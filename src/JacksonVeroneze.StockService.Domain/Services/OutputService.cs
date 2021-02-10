using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class OutputService : IOutputService
    {
        private readonly IOutputRepository _repository;
        private readonly IMediatorHandler _mediatorHandler;

        public OutputService(IOutputRepository repository, IMediatorHandler mediatorHandler)
        {
            _repository = repository;
            _mediatorHandler = mediatorHandler;
        }
    }
}
