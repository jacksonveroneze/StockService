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
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IList<PurchaseItemModel>> FindItems(Guid purchaseId)
        {
            return await Context.Set<PurchaseItem>()
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Purchase.Id == purchaseId)
                .Select(x => new PurchaseItemModel()
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Value = x.Value,
                    PurchaseId = x.Purchase.Id,
                    ProductId = x.Product.Id,
                    ProductDescription = x.Product.Description
                })
                .ToListAsync();
        }
    }
}
