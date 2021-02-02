using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Communication.Mediator;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        private readonly IMediatorHandler _mediatorHandler;

        public PurchaseService(IPurchaseRepository repository, IMediatorHandler mediatorHandler)
        {
            _repository = repository;
            _mediatorHandler = mediatorHandler;
        }

        public Task AddAsync(Purchase purchase)
        {
            return Task.CompletedTask;
        }
    }
}
