using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Data.Queries;
using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {
        }

        public Task<List<Product>> FilterAsync(Pagination pagination, ProductFilter filter)
        {
            return _context.Set<Product>()
                .AsNoTracking()
                .Where(ProductQuery.GetQuery(filter))
                .ConfigureSkipTakeFromPagination<Product>(pagination)
                .ToListAsync();
        }
    }
}
