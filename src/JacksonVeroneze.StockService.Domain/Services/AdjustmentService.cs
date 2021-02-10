using JacksonVeroneze.StockService.Bus.Mediator;
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
    }
}
