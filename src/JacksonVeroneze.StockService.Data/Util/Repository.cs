using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.StockService.Data.Util
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        public IUnitOfWork UnitOfWork => _context;

        protected readonly DatabaseContext _context;

        protected Repository(DatabaseContext context)
            => _context = context;

        public async Task AddAsync(T entity)
            => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity)
            => _context.Set<T>().Update(entity);

        public Task<List<T>> FindAllAsync()
            => _context.Set<T>().AsNoTracking().ToListAsync();

        public Task<T> FindAsync(Guid id)
            => _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);

        public void Remove(T entity)
            => _context.Set<T>().Remove(entity);
    }
}
