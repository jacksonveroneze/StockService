using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.DomainObjects;

namespace JacksonVeroneze.StockService.Core.Data
{
    public interface IRepository<T> where T : Entity
    {
        public IUnitOfWork UnitOfWork { get; set; }

        Task AddAsync(T entity);

        void Update(T entity);

        void Remove(T entity);

        Task<List<T>> FindAllAsync();

        Task<T> FindAsync(Guid id);

        Task<List<T>> FilterAsync(Expression<Func<T, bool>> filter);

        Task<List<T>> FilterAsync(Pagination pagination, Expression<Func<T, bool>> filter);
    }
}
