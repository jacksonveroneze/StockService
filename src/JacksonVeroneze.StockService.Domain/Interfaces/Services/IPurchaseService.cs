using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IPurchaseService
    {
        Task AddItem(Purchase purchase, PurchaseItem item);

        Task RemoveItem(Purchase purchase, PurchaseItem item);

        Task Close(Purchase purchase);
    }
}
