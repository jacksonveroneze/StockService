using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Repositories
{
    public interface IOutputRepository : IRepository<Output>
    {
        Task<List<Output>> FilterAsync(Pagination pagination, OutputFilter filter);
    }
}
