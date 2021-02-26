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
    public class AdjustmentRepository : Repository<Adjustment>, IAdjustmentRepository
    {
        public AdjustmentRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<List<Adjustment>> FilterAsync(Pagination pagination, AdjustmentFilter filter)
        {
            return _context.Set<Adjustment>()
                .AsNoTracking()
                .Where(AdjustmentQuery.GetQuery(filter))
                .ConfigureSkipTakeFromPagination(pagination)
                .ToListAsync();
        }
    }
}
