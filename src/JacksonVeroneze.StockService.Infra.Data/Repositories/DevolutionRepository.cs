using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Models;
using JacksonVeroneze.StockService.Infra.Data.Util;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Infra.Data.Repositories
{
    public class DevolutionRepository : Repository<Devolution>, IDevolutionRepository
    {
        public DevolutionRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IList<DevolutionItemModel>> FindItems(Guid devolutionId)
        {
            return await Context.Set<DevolutionItem>()
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Devolution.Id == devolutionId)
                .Select(x => new DevolutionItemModel()
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    DevolutionId = x.Devolution.Id,
                    ProductId = x.Product.Id,
                    ProductDescription = x.Product.Description
                })
                .ToListAsync();
        }
    }
}
