using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {

    }
}
