using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Models;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IMovementApplicationService
    {
        Task<IList<MovementModel>> FilterAsync(MovementFilter filter);
    }
}
