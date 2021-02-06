using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Core.Data
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        public IUnitOfWork UnitOfWork { get; }

        protected readonly DbContext _context;

        protected Repository(DbContext context)
            => _context = context;

        public async Task AddAsync(T entity)
            => await _context.Set<T>().AddAsync(entity);

        public Task<List<T>> FindAllAsync()
            => _context.Set<T>().AsNoTracking().ToListAsync();

        public Task<T> FindAsync(Guid id)
            => _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
    }
}
