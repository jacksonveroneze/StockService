using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Data.Util
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected int LimitDefault = 30;
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

        public Task<List<T>> FindAllAsync()
            => _context.Set<T>().AsNoTracking().ToListAsync();

        public Task<T> FindAsync(Guid id)
            => _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);

        public Task<List<T>> FilterAsync(Expression<Func<T, bool>> filter)
            => BuidQueryable(new Pagination(), filter)
                .ToListAsync();

        public Task<List<T>> FilterAsync(Pagination pagination, Expression<Func<T, bool>> filter)
            => BuidQueryable(pagination, filter)
                .ToListAsync();

        private IQueryable<T> BuidQueryable(Pagination pagination, Expression<Func<T, bool>> filter)
        {
            return _context.Set<T>()
                .AsNoTracking()
                .Where(filter)
                .OrderByDescending(x => x.Id)
                .ConfigureSkipTakeFromPagination(pagination);
        }
    }
}
