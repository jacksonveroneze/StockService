using JacksonVeroneze.StockService.Data.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Data.Repositories
{
    public class OutputRepository : Repository<Output>, IOutputRepository
    {
        public OutputRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
