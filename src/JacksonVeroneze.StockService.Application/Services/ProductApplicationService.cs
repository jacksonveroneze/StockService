using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Application.Validations.Product;
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
        private readonly IProductValidator _productValidator;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="productService"></param>
        /// <param name="productRepository"></param>
        /// <param name="productValidator"></param>
        public ProductApplicationService(IMapper mapper,
            IProductService productService,
            IProductRepository productRepository,
            IProductValidator productValidator)
        {
            _mapper = mapper;
            _productService = productService;
            _productRepository = productRepository;
            _productValidator = productValidator;
        }

        /// <summary>
        /// Method responsible for find productDto.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductDto> FindAsync(Guid productId)
            => _mapper.Map<ProductDto>(await _productRepository.FindAsync(productId));

        /// <summary>
        /// Method responsible for productFilter productDto.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="productFilter"></param>
        /// <returns></returns>
        public async Task<IList<ProductDto>> FilterAsync(Pagination pagination, ProductFilter productFilter)
            => _mapper.Map<List<ProductDto>>(
                await _productRepository.FilterAsync(pagination, productFilter));

        /// <summary>
        /// Method responsible for add productDto.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<ApplicationDataResult<ProductDto>> AddAsync(AddOrUpdateProductDto productDto)
        {
            NotificationContext result = await _productValidator.Validate(productDto);

            if (result.HasNotifications)
                return ApplicationDataResult<ProductDto>.FactoryFromNotificationContext(result);

            Product product = _mapper.Map<Product>(productDto);

            await _productService.AddAsync(product);

            return ApplicationDataResult<ProductDto>.FactoryFromData(_mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Method responsible for update productDto.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        /// <exception cref="Core.DomainObjects.Exceptions.NotFoundException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public async Task<ApplicationDataResult<ProductDto>> UpdateAsync(Guid productId, AddOrUpdateProductDto productDto)
        {
            NotificationContext result = await _productValidator.Validate(productId, productDto);

            if (result.HasNotifications)
                return ApplicationDataResult<ProductDto>.FactoryFromNotificationContext(result);

            Product product = await _productRepository.FindAsync(productId);

            product.Update(productDto.Description, productDto.IsActive);

            await _productService.UpdateAsync(product);

            return ApplicationDataResult<ProductDto>.FactoryFromData(_mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Method responsible for remove productDto.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="Core.DomainObjects.Exceptions.NotFoundException"></exception>
        public async Task<ApplicationDataResult<ProductDto>> RemoveAsync(Guid productId)
        {
            NotificationContext result = await _productValidator.Validate(productId);

            if (result.HasNotifications)
                return ApplicationDataResult<ProductDto>.FactoryFromNotificationContext(result);

            Product product = await _productRepository.FindAsync(productId);

            await _productService.RemoveAsync(product);

            return ApplicationDataResult<ProductDto>.FactoryFromEmpty();
        }
    }
}
