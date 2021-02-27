using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Movement;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IMovementApplicationService
    {
        Task<IList<MovementDto>> FilterAsync(Pagination pagination, MovementFilter filter);
    }
}
