using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IMovementService
    {
        Task AddItemAsync(Movement movement, MovementItem item);
    }
}
