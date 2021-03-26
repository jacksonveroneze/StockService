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

        protected readonly DatabaseContext _context;

        protected Repository(DatabaseContext context)
        {
            _context = context;
            UnitOfWork = _context;
        }

        public async Task AddAsync(T entity)
            => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity)
            => _context.Set<T>().Update(entity);

        public void Remove(T entity)
            => _context.Set<T>().Remove(entity);

        public Task<T> FindAsync(Guid id)
            => EF.CompileAsyncQuery((DatabaseContext context, Guid idInner) =>
                context.Set<T>()
                    //.Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(5))
                    .FirstOrDefault(c => c.Id == idInner)).Invoke(_context, id);

        public Task<T> FindAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<T>
            => BuidQueryable(new Pagination(), filter)
                .FirstOrDefaultAsync();

        public Task<List<T>> FilterAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<T>
            => BuidQueryable(new Pagination(), filter)
                .ToListAsync();

        public Task<List<T>> FilterAsync<TFilter>(Pagination pagination, TFilter filter) where TFilter : BaseFilter<T>
            => BuidQueryable(pagination, filter)
                .ToListAsync();

        public async Task<Pageable<T>> FilterPaginateAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<T>
        {
            IQueryable<T> query = _context.Set<T>()
                .AsNoTracking()
                .Where(filter.ToQuery());

            int total = await query.CountAsync();

            List<T> data = await BuidQueryable(pagination, filter)
                .ToListAsync();

            return new Pageable<T>
            {
                Data = data,
                Total = total,
                Pages = (int)Math.Ceiling(total / (decimal)(pagination.Take ??= 30)),
                CurrentPage = pagination.Skip <= 0 ? 1 : pagination.Skip
            };
        }

        private IQueryable<T> BuidQueryable<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<T>
        {
            return _context.Set<T>()
                .Where(filter.ToQuery())
                .OrderByDescending(x => x.CreatedAt)
                .ConfigureSkipTakeFromPagination(pagination);
        }
    }
}
