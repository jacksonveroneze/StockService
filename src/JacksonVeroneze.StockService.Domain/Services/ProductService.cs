using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Domain.Services
{
    /// <summary>
    ///
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="repository"></param>
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Method responsible for add item.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task AddAsync(Product product)
        {
            await _repository.AddAsync(product);

            await _repository.UnitOfWork.CommitAsync();
        }
    }
}
