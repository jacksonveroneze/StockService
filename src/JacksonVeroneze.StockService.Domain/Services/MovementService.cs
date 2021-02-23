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

        public async Task AddItemAsync(Movement movement, MovementItem item)
        {
            movement.AddItem(item);

            _repository.Update(movement);

            await _repository.UnitOfWork.CommitAsync();
        }
    }
}
