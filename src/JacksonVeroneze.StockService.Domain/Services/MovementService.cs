using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class MovementService : IMovementService
    {
        private readonly IMovementRepository _repository;

        public MovementService(IMovementRepository repository)
            => _repository = repository;

        public Task AddItemAsync(Movement movement, MovementItem item) => throw new System.NotImplementedException();
    }
}
