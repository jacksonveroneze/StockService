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
        private readonly DbSet<T> _dbSet;

        protected readonly DatabaseContext Context;

        public IUnitOfWork UnitOfWork { get; set; }

        protected Repository(DatabaseContext context)
        {
            _dbSet = context.Set<T>();
            Context = context;
            UnitOfWork = Context;
        }

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Remove(T entity)
            => _dbSet.Remove(entity);

        public Task<T> FindAsync(Guid id)
            => _dbSet.FirstOrDefaultAsync(c => c.Id == id);

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
            => _dbSet
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
            return _dbSet
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
