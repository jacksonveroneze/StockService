using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.Product
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class ProductValidator : Validator, IProductValidator
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Method responsible for initialize validator.
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductValidator(IProductRepository productRepository)
            => _productRepository = productRepository;

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(AddOrUpdateProductDto productDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDTOAsync(notificationContext, productDto);

            if (string.IsNullOrEmpty(productDto.Description)) return notificationContext;

            Domain.Entities.Product result =
                await _productRepository.FindAsync(new ProductFilter {Description = productDto.Description});

            if (result != null)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Product), ApplicationValidationMessages.ProductFoundByDescription));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateUpdateAsync(Guid productId, AddOrUpdateProductDto productDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDTOAsync(notificationContext, productDto);

            Domain.Entities.Product product = await _productRepository.FindAsync(productId);

            if (product is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Product), ApplicationValidationMessages.ProductNotFoundById));

            if (string.IsNullOrEmpty(productDto.Description)) return notificationContext;

            Domain.Entities.Product result =
                await _productRepository.FindAsync(new ProductFilter {Description = productDto.Description});

            if (result != null && result.Id != productId)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Product), ApplicationValidationMessages.ProductFoundByDescription));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateRemoveAsync(Guid productId)
        {
            NotificationContext notificationContext = new();

            Domain.Entities.Product product = await _productRepository.FindAsync(productId);

            if (product is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Product), ApplicationValidationMessages.ProductNotFoundById));

            if (product.HasItems)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Product), ApplicationValidationMessages.ProductHasItems));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        private async Task ValidateDTOAsync(NotificationContext notificationContext, AddOrUpdateProductDto productDto)
        {
            ValidationResult validationResult = await productDto.Validate();

            notificationContext.AddNotifications(validationResult);
        }
    }
}
