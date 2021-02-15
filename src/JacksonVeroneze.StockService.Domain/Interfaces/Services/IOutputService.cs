using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Services
{
    public interface IOutputService
    {
        Task AddItem(Output output, OutputItem item);

        Task UpdateItem(Output output, OutputItem item);

        Task RemoveItem(Output output, OutputItem item);

        Task Close(Output output);
    }
}
