using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Models;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Repositories
{
    public interface IMovementRepository : IRepository<Movement>
    {
        Task<Pageable<MovementModel>> ReportFilterAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<Movement>;
    }
}
