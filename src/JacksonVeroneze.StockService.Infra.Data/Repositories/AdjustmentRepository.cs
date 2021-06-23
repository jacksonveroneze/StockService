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
    public class AdjustmentRepository : Repository<Adjustment>, IAdjustmentRepository
    {
        public AdjustmentRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IList<AdjustmentItemModel>> FindItems(Guid adjustmentId)
        {
            return await Context.Set<AdjustmentItem>()
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Adjustment.Id == adjustmentId)
                .Select(x => new AdjustmentItemModel()
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    AdjustmentId = x.Adjustment.Id,
                    ProductId = x.Product.Id,
                    ProductDescription = x.Product.Description
                })
                .ToListAsync();
        }
    }
}
