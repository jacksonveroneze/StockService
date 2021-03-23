using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Infra.Data.Util;

namespace JacksonVeroneze.StockService.Infra.Data.Repositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
