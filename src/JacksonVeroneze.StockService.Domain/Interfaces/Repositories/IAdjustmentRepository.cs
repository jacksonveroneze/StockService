using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Repositories
{
    public interface IAdjustmentRepository : IRepository<Adjustment>
    {
        Task<List<Adjustment>> FilterAsync(Pagination pagination, AdjustmentFilter filter);
    }
}
