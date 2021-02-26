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
    public class OutputRepository : Repository<Output>, IOutputRepository
    {
        public OutputRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<List<Output>> FilterAsync(Pagination pagination, OutputFilter filter)
        {
            return _context.Set<Output>()
                .AsNoTracking()
                .Where(OutputQuery.GetQuery(filter))
                .ConfigureSkipTakeFromPagination(pagination)
                .ToListAsync();
        }
    }
}
