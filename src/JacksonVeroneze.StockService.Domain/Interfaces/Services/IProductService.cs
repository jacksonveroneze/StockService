using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task RemoveAsync(Product product);
    }
}
