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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<List<Product>> FilterAsync(ProductFilter filter)
            => base.FilterAsync(ProductQuery.GetQuery(filter));

        public Task<List<Product>> FilterAsync(Pagination pagination, ProductFilter filter)
            => base.FilterAsync(pagination, ProductQuery.GetQuery(filter));
    }
}
