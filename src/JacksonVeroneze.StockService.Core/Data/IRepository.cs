using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Core.Data
{
    public interface IRepository<T> where T : Entity
    {
        public IUnitOfWork UnitOfWork { get; }

        Task AddAsync(T entity);

        Task<List<T>> FindAllAsync();

        Task<T> FindAsync(Guid id);

        void RemoveAsync(T entity);
    }
}
