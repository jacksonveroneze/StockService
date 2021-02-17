using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public Task<List<Product>> FilterAsync(ProductFilter filter)
        {
            Expression<Func<Product, bool>> expression =
                x => x.Description.Contains(filter.Description);

            return _context.Set<Product>()
                .Where(expression)
                .ToListAsync();
        }
    }
}
