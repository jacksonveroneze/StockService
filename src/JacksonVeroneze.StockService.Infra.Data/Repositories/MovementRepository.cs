using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Models;
using JacksonVeroneze.StockService.Infra.Data.Util;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Infra.Data.Repositories
{
    public class MovementRepository : Repository<Movement>, IMovementRepository
    {
        public MovementRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<List<MovementModel>> ReportFilterAsync<TFilter>(TFilter filter)
            where TFilter : BaseFilter<Movement>
        {
            return await _context.Set<Movement>()
                .Include(x => x.Items)
                .Include(x => x.Product)
                .Where(filter.ToQuery())
                .Select(x => new MovementModel()
                {
                    ProductId = x.Product.Id,
                    ProductDescription = x.Product.Description,
                    Ammount = x.FindLastAmmount().Value
                })
                .ToListAsync();
        }
    }
}
