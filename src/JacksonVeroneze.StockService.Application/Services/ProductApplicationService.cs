using System;
using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Application.Validations.Product;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.Notifications;
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
        /// Method responsible for find product.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductDto> FindAsync(Guid productId)
            => _mapper.Map<ProductDto>(await _productRepository.FindAsync(productId));

        /// <summary>
        /// Method responsible for find list of products.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="productFilter"></param>
        /// <returns></returns>
        public async Task<Pageable<ProductDto>> FilterAsync(Pagination pagination, ProductFilter productFilter)
            => _mapper.Map<Pageable<ProductDto>>(
                await _productRepository.FilterPaginateAsync(pagination, productFilter));

        /// <summary>
        /// Method responsible for add product.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<ProductDto>> AddAsync(AddOrUpdateProductDto productDto)
        {
            NotificationContext result = await _productValidator.ValidateCreateAsync(productDto);

            if (result.HasNotifications)
                return ApplicationDataResult<ProductDto>.FactoryFromNotificationContext(result);

            Product product = _mapper.Map<Product>(productDto);

            await _productService.AddAsync(product);

            return ApplicationDataResult<ProductDto>.FactoryFromData(_mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Method responsible for update product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<ProductDto>> UpdateAsync(Guid productId,
            AddOrUpdateProductDto productDto)
        {
            NotificationContext result = await _productValidator.ValidateUpdateAsync(productId, productDto);

            if (result.HasNotifications)
                return ApplicationDataResult<ProductDto>.FactoryFromNotificationContext(result);

            Product product = await _productRepository.FindAsync(productId);

            product.Update(productDto.Description, productDto.IsActive);

            _productRepository.Update(product);

            await _productRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<ProductDto>.FactoryFromData(_mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Method responsible for remove product.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<ProductDto>> RemoveAsync(Guid productId)
        {
            NotificationContext result = await _productValidator.ValidateRemoveAsync(productId);

            if (result.HasNotifications)
                return ApplicationDataResult<ProductDto>.FactoryFromNotificationContext(result);

            Product product = await _productRepository.FindAsync(productId);

            _productRepository.Remove(product);

            await _productRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<ProductDto>.FactoryFromEmpty();
        }
    }
}
