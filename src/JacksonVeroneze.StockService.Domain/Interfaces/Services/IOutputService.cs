using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IOutputService
    {
        Task AddItemAsync(Output output, OutputItem item);

        Task UpdateItemAsync(Output output, OutputItem item);

        Task RemoveItemAsync(Output output, OutputItem item);

        Task CloseAsync(Output output);

        Task UndoItemAsync(Output output, OutputItem item);
    }
}
