using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Data.Repositories
{
    public class MovementRepository : Repository<Movement>, IMovementRepository
    {
        public MovementRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<Movement> FindByProductIdAsync(Guid productId)
        {
            return _context.Set<Movement>()
                .FirstOrDefaultAsync(x => x.Product.Id == productId);
        }
    }
}
