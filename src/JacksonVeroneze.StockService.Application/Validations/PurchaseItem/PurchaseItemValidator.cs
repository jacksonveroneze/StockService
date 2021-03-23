using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.PurchaseItem
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class PurchaseItemValidator : Validator, IPurchaseItemValidator
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
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(Guid purchaseId, AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDefaultActionsAsync(notificationContext, purchaseId, default);
            await ValidateDtoAsync(notificationContext, purchaseItemDto);

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

            await ValidateDefaultActionsAsync(notificationContext, purchaseId, purchaseItemId);
            await ValidateDtoAsync(notificationContext, purchaseItemDto);

            return notificationContext;
        }

        /// <summary>
        ///  Method responsible for validation.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateRemoveAsync(Guid purchaseId, Guid purchaseItemId)
        {
            NotificationContext notificationContext = new();

            await ValidateDefaultActionsAsync(notificationContext, purchaseId, purchaseItemId);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="purchaseItemDto"></param>
        /// <returns></returns>
        private async Task ValidateDtoAsync(NotificationContext notificationContext,
            AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            ValidationResult validationResult = await purchaseItemDto.Validate();

            notificationContext.AddNotifications(validationResult);

            Domain.Entities.Product product = await _productRepository.FindAsync(purchaseItemDto.ProductId);

            if (product is null)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.ProductNotFoundById));
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemId"></param>
        /// <returns></returns>
        private async Task ValidateDefaultActionsAsync(NotificationContext notificationContext, Guid purchaseId,
            Guid? purchaseItemId)
        {
            Domain.Entities.Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
            {
                notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseNotFoundById));

                return;
            }

            if (purchase.State == PurchaseState.Closed)
            {
                notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseIsClosed));

                return;
            }

            if (purchaseItemId.HasValue is false) return;

            Domain.Entities.PurchaseItem purchaseItem = purchase.FindItem(purchaseItemId.Value);

            if (purchaseItem is null)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseItemNotFoundById));
        }
    }
}
