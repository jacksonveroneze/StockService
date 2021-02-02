using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces
{
    public interface IPurchaseService
    {
        Task AddAsync(Purchase purchase);
    }
}
