using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Data.Queries;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Util;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Data.Repositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<List<Purchase>> FilterAsync(Pagination pagination, PurchaseFilter filter)
        {
            return _context.Set<Purchase>()
                .AsNoTracking()
                .Where(PurchaseQuery.GetQuery(filter))
                .ConfigureSkipTakeFromPagination(pagination)
                .ToListAsync();
        }
    }
}
