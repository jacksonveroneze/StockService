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
    public class ProductValidator : IProductValidator
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

            ValidationResult validationResult = await productDto.Validate();

            notificationContext.AddNotifications(validationResult);

            if (string.IsNullOrEmpty(productDto.Description) is false)
            {
                Domain.Entities.Product result =
                    await _productRepository.FindAsync(new ProductFilter {Description = productDto.Description});

                if (result != null)
                    notificationContext.AddNotification(new Notification("product",
                        ApplicationValidationMessages.ProductFound));
            }

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

            ValidationResult validationResult = await productDto.Validate();

            notificationContext.AddNotifications(validationResult);

            await ValidateIfExistsProduct(productId, notificationContext);

            if (string.IsNullOrEmpty(productDto.Description) is false)
            {
                Domain.Entities.Product result =
                    await _productRepository.FindAsync(new ProductFilter {Description = productDto.Description});

                if (result != null && result.Id != productId)
                    notificationContext.AddNotification(new Notification("product",
                        "Produto já cadastrado com a descrição informada"));
            }

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

            await ValidateIfExistsProduct(productId, notificationContext);

            Domain.Entities.Product product = await _productRepository.FindAsync(productId);

            if (product != null && product.HasItems)
                notificationContext.AddNotification(new Notification("product",
                    "Este produto tem dependencias, portanto não pode ser removido."));

            return notificationContext;
        }

        private async Task ValidateIfExistsProduct(Guid productId,
            NotificationContext notificationContext)
        {
            Domain.Entities.Product product = await _productRepository.FindAsync(productId);

            if (product is null)
                notificationContext.AddNotification(new Notification("product",
                    "Produto näo encontrado com o código informado"));
        }
    }
}
