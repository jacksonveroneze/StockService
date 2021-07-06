using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Models;
using JacksonVeroneze.StockService.Domain.Util;
using JacksonVeroneze.StockService.Infra.Data.Util;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Infra.Data.Repositories
{
    public class MovementRepository : Repository<Movement>, IMovementRepository
    {
        public MovementRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<Pageable<MovementModel>> ReportFilterAsync<TFilter>(TFilter filter)
            where TFilter : BaseFilter<Movement>
        {
            int total = await CountAsync(filter);

            List<MovementModel> data = await Context.Set<Movement>()
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .Where(filter.ToQuery())
                .Select(x => new MovementModel()
                {
                    ProductId = x.Product.Id,
                    ProductDescription = x.Product.Description,
                    Ammount = x.Items.OrderByDescending(b => b.CreatedAt).FirstOrDefault().Amount
                })
                .ToListAsync();

            return FactoryPageable(data, total, 0, total);
        }

        public Task<MovementModel> FindByProductAsync(Guid productId)
        {
            return Context.Set<Movement>()
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Product.Id == productId)
                .Select(x => new MovementModel()
                {
                    ProductId = x.Product.Id,
                    ProductDescription = x.Product.Description,
                    Ammount = x.Items.OrderByDescending(b => b.CreatedAt).FirstOrDefault().Amount
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IList<MovementItem>> FindItensToRecalcOnUndoOutputItemAsync(Guid productId,
            DateTime startDate,
            DateTime? endDate)
        {
            Expression<Func<MovementItem, bool>> expression = x
                => x.Movement.Product.Id == productId && (x.OutputItems.Any() || x.PurchaseItems.Any()) &&
                   x.CreatedAt > startDate;

            if (endDate.HasValue)
                expression = expression.And(b => b.CreatedAt < endDate);

            return await Context.Set<MovementItem>()
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .Where(expression)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<MovementItem> FindFirstAjustment(Guid movementId, DateTime startDate)
        {
            return await Context.Set<MovementItem>()
                .AsSplitQuery()
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Movement.Id == movementId && x.CreatedAt > startDate &&
                            x.AdjustmentItems.Any())
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
