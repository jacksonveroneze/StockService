using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Infra.Data.Util
{
    public class Repository<T> : IRepository<T> where T : EntityRoot
    {
        public IUnitOfWork UnitOfWork { get; set; }

        protected readonly DatabaseContext Context;

        protected Repository(DatabaseContext context)
        {
            Context = context;
            UnitOfWork = Context;
        }

        public async Task AddAsync(T entity)
            => await Context.Set<T>().AddAsync(entity);

        public void Update(T entity)
            => Context.Set<T>().Update(entity);

        public void Remove(T entity)
            => Context.Set<T>().Remove(entity);

        public Task<T> FindAsync(Guid id)
            => EF.CompileAsyncQuery((DatabaseContext context, Guid idInner) =>
                context.Set<T>()
                    .FirstOrDefault(c => c.Id == idInner)).Invoke(Context, id);

        public Task<T> FindAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<T>
            => BuidQueryable(new Pagination(), filter)
                .FirstOrDefaultAsync();

        public Task<List<T>> FilterAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<T>
            => BuidQueryable(new Pagination(), filter)
                .ToListAsync();

        public Task<List<T>> FilterAsync<TFilter>(Pagination pagination, TFilter filter) where TFilter : BaseFilter<T>
            => BuidQueryable(pagination, filter)
                .ToListAsync();

        protected Task<int> CountAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<T>
            => Context.Set<T>()
                .AsNoTracking()
                .Where(filter.ToQuery())
                .CountAsync();

        public async Task<Pageable<T>> FilterPaginateAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<T>
        {
            int total = await CountAsync(filter);

            List<T> data = await BuidQueryable(pagination, filter).ToListAsync();

            return FactoryPageable(data, total, pagination.Skip ??= 0, pagination.Take ??= 30);
        }

        private IQueryable<T> BuidQueryable<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<T>
        {
            return Context.Set<T>()
                .Where(filter.ToQuery())
                .OrderByDescending(x => x.CreatedAt)
                .ConfigureSkipTakeFromPagination(pagination);
        }

        protected Pageable<TType> FactoryPageable<TType>(IList<TType> data, int total, int skip, int take)
            where TType : class
        {
            return new()
            {
                Data = data,
                Total = total,
                Pages = total > 0 ? (int)Math.Ceiling(total / (decimal)(take)) : 0,
                CurrentPage = skip <= 0 ? 1 : skip
            };
        }
    }
}
