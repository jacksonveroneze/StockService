using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.Product
{
    public class ProductValidator : IProductValidator
    {
        private readonly IProductRepository _productRepository;

        public ProductValidator(IProductRepository productRepository)
            => _productRepository = productRepository;

        public async Task<NotificationContext> Validate(AddOrUpdateProductDto productDto)
        {
            NotificationContext notificationContext = new();

            ValidationResult validationResult = await productDto.Validate();

            notificationContext.AddNotifications(validationResult);

            Domain.Entities.Product result =
                await _productRepository.FindAsync(new ProductFilter {Description = productDto.Description});

            if (result != null)
                notificationContext.AddNotification(new Notification("product", "Produto já cadastrado"));

            return notificationContext;
        }

        public async Task<NotificationContext> Validate(Guid productId)
        {
            NotificationContext notificationContext = new();

            await ValidateIfExistsProduct(productId, notificationContext);

            return notificationContext;
        }

        public async Task<NotificationContext> Validate(Guid productId, AddOrUpdateProductDto productDto)
        {
            NotificationContext notificationContext = new();

            ValidationResult validationResult = await productDto.Validate();

            notificationContext.AddNotifications(validationResult);

            await ValidateIfExistsProduct(productId, notificationContext);

            return notificationContext;
        }

        private async Task ValidateIfExistsProduct(Guid productId,
            NotificationContext notificationContext)
        {
            Domain.Entities.Product product = await _productRepository.FindAsync(productId);

            if (product is null)
                notificationContext.AddNotification(new Notification("product", "Produto näo encontrado"));
        }
    }
}
