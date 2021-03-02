using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class ProductApplicationService : ApplicationService, IProductApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IValidator<AddOrUpdateProductDto> _validatorProduct;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="productService"></param>
        /// <param name="productRepository"></param>
        /// <param name="validatorProduct"></param>
        public ProductApplicationService(IMapper mapper, IProductService productService,
            IProductRepository productRepository,
            IValidator<AddOrUpdateProductDto> validatorProduct)
        {
            _mapper = mapper;
            _productService = productService;
            _productRepository = productRepository;
            _validatorProduct = validatorProduct;
        }

        /// <summary>
        /// Method responsible for find data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductDto> FindAsync(Guid id)
            => _mapper.Map<ProductDto>(await _productRepository.FindAsync(id));

        /// <summary>
        /// Method responsible for filter data.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IList<ProductDto>> FilterAsync(Pagination pagination, ProductFilter filter)
            => _mapper.Map<List<ProductDto>>(
                await _productRepository.FilterAsync(pagination, filter));

        /// <summary>
        /// Method responsible for add data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<ApplicationDataResult<ProductDto>> AddAsync(AddOrUpdateProductDto data)
        {
            ValidationResult validationResult = await _validatorProduct.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return FactoryFromValidationResult<ProductDto>(validationResult);

            Product product = _mapper.Map<Product>(data);

            await _productService.AddAsync(product);

            return FactoryResultFromData(_mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Method responsible for update data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Core.DomainObjects.Exceptions.NotFoundException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public async Task<ApplicationDataResult<ProductDto>> UpdateAsync(Guid id, AddOrUpdateProductDto data)
        {
            ValidationResult validationResult = await _validatorProduct.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return FactoryFromValidationResult<ProductDto>(validationResult);

            Product product = await _productRepository.FindAsync(id);

            if (product is null)
                throw ExceptionsFactory.FactoryNotFoundException<Product>(id);

            await _productService.UpdateAsync(product);

            return FactoryResultFromData(_mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Method responsible for remove data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Core.DomainObjects.Exceptions.NotFoundException"></exception>
        public async Task RemoveAsync(Guid id)
        {
            Product product = await _productRepository.FindAsync(id);

            if (product is null)
                throw ExceptionsFactory.FactoryNotFoundException<Product>(id);

            await _productService.RemoveAsync(product);
        }
    }
}
