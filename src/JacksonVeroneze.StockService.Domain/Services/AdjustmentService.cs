using JacksonVeroneze.StockService.Bus.Mediator;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class AdjustmentService : IAdjustmentService
    {
        private readonly IAdjustmentRepository _repository;
        private readonly IMediatorHandler _mediatorHandler;

        public AdjustmentService(IAdjustmentRepository repository, IMediatorHandler mediatorHandler)
        {
            _repository = repository;
            _mediatorHandler = mediatorHandler;
        }
    }
}
