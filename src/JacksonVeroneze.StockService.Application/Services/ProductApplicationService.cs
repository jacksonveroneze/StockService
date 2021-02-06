using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Domain.Interfaces;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IProductRepository _productRepository;

        public ProductApplicationService(IProductRepository productRepository)
            => _productRepository = productRepository;

        public Task AddASync(ProductDto productDto) => throw new NotImplementedException();

        public Task UpdateASync(ProductDto productDto) => throw new NotImplementedException();

        public Task RemoveASync(Guid productId) => throw new NotImplementedException();
    }
}
