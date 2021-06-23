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
    public class OutputRepository : Repository<Output>, IOutputRepository
    {
        public OutputRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IList<OutputItemModel>> FindItems(Guid outputId)
        {
            return await Context.Set<OutputItem>()
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Output.Id == outputId)
                .Select(x => new OutputItemModel()
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    OutputId = x.Output.Id,
                    ProductId = x.Product.Id,
                    ProductDescription = x.Product.Description
                })
                .ToListAsync();
        }
    }
}
