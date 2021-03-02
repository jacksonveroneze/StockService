using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Bus;
using JacksonVeroneze.StockService.Core;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IBus _bus;

        public ProductService(IProductRepository repository, IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        public async Task AddAsync(Product product)
        {
            Product result = await _repository.FindAsync(new ProductFilter {Description = product.Description});

            if (result != null)
                throw ExceptionsFactory.FactoryDomainException(Messages.ProductFound);

            await _repository.AddAsync(product);

            await _repository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            Product result = await _repository.FindAsync(new ProductFilter {Description = product.Description});

            if (result != null && result.Id != product.Id)
                throw ExceptionsFactory.FactoryDomainException(Messages.ProductFound);

            _repository.Update(product);

            await _repository.UnitOfWork.CommitAsync();
        }

        public async Task RemoveAsync(Product product)
        {
            if (product.ItemsAdjustment.Any() || product.ItemsMovement.Any() || product.ItemsOutput.Any() ||
                product.ItemsPurchase.Any())
                throw ExceptionsFactory.FactoryDomainException(
                    "Este produtos tem dependentes, portanto não pode ser excluido.");

            _repository.Remove(product);

            await _repository.UnitOfWork.CommitAsync();
        }
    }
}
