using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public ValidationResult ValidationResult { get; set; }


        public ProductApplicationService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductDto> AddASync(ProductDto productDto)
        {
            ValidationResult = await new ProductDtoValidator()
                .ValidateAsync(productDto);

            if (ValidationResult.IsValid is false)
                return null;

            Product product = _mapper.Map<Product>(productDto);

            await _productRepository.AddAsync(product);

            await _productRepository.UnitOfWork.CommitAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public Task UpdateASync(ProductDto productDto) => throw new NotImplementedException();

        public Task RemoveASync(Guid productId) => throw new NotImplementedException();
    }
}
