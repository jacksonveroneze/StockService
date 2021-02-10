using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Data.Repositories
{
    public class AdjustmentRepository : Repository<Adjustment>, IAdjustmentRepository
    {
        public AdjustmentRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
