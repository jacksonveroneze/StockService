using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IPurchaseService
    {
        Task AddAsync(Purchase purchase);
        
        Task AddItemAsync(Purchase purchase, PurchaseItem item);

        Task UpdateItemAsync(Purchase purchase, PurchaseItem item);

        Task RemoveItemAsync(Purchase purchase, PurchaseItem item);

        Task CloseAsync(Purchase purchase);
    }
}
