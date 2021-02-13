using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductApplicationService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ProductDto> FindAsync(Guid id)
            => _mapper.Map<ProductDto>(await _productRepository.FindAsync(id));

        public async Task<IEnumerable<ProductDto>> FindAllAsync()
            => _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.FindAllAsync());

        public async Task<IEnumerable<ProductDto>> FilterAsync(ProductFilter filter)
            => _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.FilterAsync(filter));

        public async Task<ApplicationDataResult<ProductDto>> AddASync(AddOrUpdateProductDto data)
        {
            ValidationResult validationResult = await data.Validate();

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<ProductDto>(validationResult.Errors.Select(x => x.ErrorMessage));

            Product product = _mapper.Map<Product>(data);

            await _productRepository.AddAsync(product);

            await _productRepository.UnitOfWork.CommitAsync();

            return new ApplicationDataResult<ProductDto>(_mapper.Map<ProductDto>(product));
        }

        public Task<ApplicationDataResult<ProductDto>> UpdateASync(AddOrUpdateProductDto data) => throw new NotImplementedException();

        public async Task RemoveASync(Guid id)
        {
            Product product = await _productRepository.FindAsync(id);

            _productRepository.Remove(product);

            await _productRepository.UnitOfWork.CommitAsync();
        }
    }
}
