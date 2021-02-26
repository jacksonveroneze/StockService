using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IValidator<AddOrUpdateProductDto> _validatorProduct;

        public ProductApplicationService(IMapper mapper, IProductRepository productRepository,
            IValidator<AddOrUpdateProductDto> validatorProduct)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _validatorProduct = validatorProduct;
        }

        public async Task<ProductDto> FindAsync(Guid id)
            => _mapper.Map<ProductDto>(await _productRepository.FindAsync(id));

        public async Task<IList<ProductDto>> FilterAsync(Pagination pagination, ProductFilter filter)
            => _mapper.Map<List<ProductDto>>(
                await _productRepository.FilterAsync(pagination, filter));

        public async Task<ApplicationDataResult<ProductDto>> AddASync(AddOrUpdateProductDto data)
        {
            ValidationResult validationResult = await _validatorProduct.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<ProductDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Product product = _mapper.Map<Product>(data);

            await _productRepository.AddAsync(product);

            await _productRepository.UnitOfWork.CommitAsync();

            return new ApplicationDataResult<ProductDto>(_mapper.Map<ProductDto>(product));
        }

        public async Task<ApplicationDataResult<ProductDto>> UpdateASync(Guid id, AddOrUpdateProductDto data)
        {
            ValidationResult validationResult = await _validatorProduct.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<ProductDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Product product = await _productRepository.FindAsync(id);

            if (product is null)
                throw new DomainException(Messages.ItemNotFound);

            product.Update(data.Description, data.IsActive);

            _productRepository.Update(product);

            await _productRepository.UnitOfWork.CommitAsync();

            return new ApplicationDataResult<ProductDto>(_mapper.Map<ProductDto>(product));
        }

        public async Task RemoveASync(Guid id)
        {
            Product product = await _productRepository.FindAsync(id);

            if (product is null)
                throw new DomainException(Messages.ItemNotFound);

            _productRepository.Remove(product);

            await _productRepository.UnitOfWork.CommitAsync();
        }
    }
}
