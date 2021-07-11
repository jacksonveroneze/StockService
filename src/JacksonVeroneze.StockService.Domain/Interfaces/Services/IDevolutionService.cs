using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IDevolutionService
    {
        Task AddAsync(Devolution devolution);

        Task AddItemAsync(Devolution devolution,DevolutionItem item);

        Task UpdateItemAsync(Devolution devolution, DevolutionItem item);

        Task RemoveItemAsync(Devolution devolution, DevolutionItem item);

        Task CloseAsync(Devolution devolution);
    }
}
