using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Infra.Data.Util;

namespace JacksonVeroneze.StockService.Infra.Data.Repositories
{
    public class AdjustmentRepository : Repository<Adjustment>, IAdjustmentRepository
    {
        public AdjustmentRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
