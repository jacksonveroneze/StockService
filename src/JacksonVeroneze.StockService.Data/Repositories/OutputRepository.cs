using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Data.Queries;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Data.Repositories
{
    public class OutputRepository : Repository<Output>, IOutputRepository
    {
        public OutputRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<List<Output>> FilterAsync(Pagination pagination, OutputFilter filter)
            => base.FilterAsync(pagination, OutputQuery.GetQuery(filter));
    }
}
