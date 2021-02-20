using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Repositories
{
    public interface IMovementRepository : IRepository<Movement>
    {
        Task<Movement> FindByProductIdAsync(Guid productId);
    }
}
