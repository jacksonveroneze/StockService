using System.Threading.Tasks;

namespace JacksonVeroneze.StockService.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
