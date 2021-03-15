using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    /// <summary>
    /// Method responsible for service.
    /// </summary>
    public class MovementService : IMovementService
    {
        private readonly IMovementRepository _repository;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="repository"></param>
        public MovementService(IMovementRepository repository)
            => _repository = repository;

        /// <summary>
        /// Method responsible for add item.
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddItemAsync(Movement movement, MovementItem item)
        {
            movement.AddItem(item);

            _repository.Update(movement);

            await _repository.UnitOfWork.CommitAsync();
        }
    }
}
