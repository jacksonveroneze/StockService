using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IAdjustmentService
    {
        Task AddAsync(Adjustment adjustment);

        Task AddItemAsync(Adjustment adjustment, AdjustmentItem item);

        Task UpdateItemAsync(Adjustment adjustment, AdjustmentItem item);

        Task RemoveItemAsync(Adjustment adjustment, AdjustmentItem item);

        Task CloseAsync(Adjustment adjustment);
    }
}
