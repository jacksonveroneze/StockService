using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IAdjustmentService
    {
        Task AddItem(Adjustment adjustment, AdjustmentItem item);

        Task UpdateItem(Adjustment adjustment, AdjustmentItem item);

        Task RemoveItem(Adjustment adjustment, AdjustmentItem item);

        Task Close(Adjustment adjustment);
    }
}
