using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Models;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IMovementApplicationService
    {
        Task<Pageable<MovementModel>> FilterAsync(MovementFilter filter);

        Task<MovementModel> FindByProductAsync(Guid productId);
    }
}
