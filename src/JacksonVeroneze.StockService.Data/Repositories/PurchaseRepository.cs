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
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<List<Purchase>> FilterAsync(Pagination pagination, PurchaseFilter filter)
            => base.FilterAsync(pagination, PurchaseQuery.GetQuery(filter));
    }
}
