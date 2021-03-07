using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.PurchaseItem
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class PurchaseItemValidator : IPurchaseItemValidator
    {
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        /// <summary>
        /// Method responsible for initialize validator.
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="productRepository1"></param>
        public PurchaseItemValidator(IPurchaseRepository productRepository, IProductRepository productRepository1)
        {
            _purchaseRepository = productRepository;
            _productRepository = productRepository1;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="purchaseItemDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            NotificationContext notificationContext = new();

            await ValidatePurchaseItemDto(notificationContext, purchaseItemDto);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemId"></param>
        /// <param name="purchaseItemDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateUpdateAsync(Guid purchaseId, Guid purchaseItemId,
            AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            NotificationContext notificationContext = new();

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateRemoveAsync(Guid purchaseId, Guid purchaseItemId)
        {
            NotificationContext notificationContext = new();

            await ValidateIfExistPurchase(notificationContext, purchaseId);
            await ValidateIfExistPurchaseItem(notificationContext, purchaseId, purchaseItemId);

            return notificationContext;
        }

        private async Task ValidateIfExistPurchase(NotificationContext notificationContext, Guid purchaseId)
        {
            Domain.Entities.Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                notificationContext.AddNotification(new Notification("purchase", "Compra näo encontrado"));
        }

        private async Task ValidateIfExistPurchaseItem(NotificationContext notificationContext, Guid purchaseId, Guid purchaseItemId)
        {
            Domain.Entities.Purchase product = await _purchaseRepository.FindAsync(purchaseId);

            if (product.FindItem(purchaseItemId) is null)
                notificationContext.AddNotification(new Notification("purchaseItem", "Item da compra näo encontrado"));
        }

        private async Task ValidateIfExistsProduct(NotificationContext notificationContext, Guid productId)
        {
            Domain.Entities.Product product = await _productRepository.FindAsync(productId);

            if (product is null)
                notificationContext.AddNotification(new Notification("product", "O produto informado não foi encontrado"));
        }

        private async Task ValidatePurchaseItemDto(NotificationContext notificationContext,
            AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            ValidationResult validationResult = await purchaseItemDto.Validate();

            await ValidateIfExistsProduct(notificationContext, purchaseItemDto.ProductId);

            notificationContext.AddNotifications(validationResult);
        }
    }
}
