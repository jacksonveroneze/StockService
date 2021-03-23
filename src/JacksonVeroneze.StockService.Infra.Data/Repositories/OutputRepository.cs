using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Infra.Data.Util;

namespace JacksonVeroneze.StockService.Infra.Data.Repositories
{
    public class OutputRepository : Repository<Output>, IOutputRepository
    {
        public OutputRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
