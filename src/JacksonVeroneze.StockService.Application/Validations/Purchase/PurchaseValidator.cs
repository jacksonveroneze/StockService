using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.Purchase
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class PurchaseValidator : Validator, IPurchaseValidator
    {
        private readonly IPurchaseRepository _purchaseRepository;

        /// <summary>
        /// Method responsible for initialize validator.
        /// </summary>
        /// <param name="purchaseRepository"></param>
        public PurchaseValidator(IPurchaseRepository purchaseRepository)
            => _purchaseRepository = purchaseRepository;

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(AddOrUpdatePurchaseDto purchaseDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDtoAsync(notificationContext, purchaseDto);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateUpdateAsync(Guid purchaseId, AddOrUpdatePurchaseDto purchaseDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDtoAsync(notificationContext, purchaseDto);

            Domain.Entities.Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseNotFoundById));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateRemoveAsync(Guid purchaseId)
        {
            NotificationContext notificationContext = new();

            Domain.Entities.Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseNotFoundById));

            if (purchase.HasItems)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseHasItems));

            if (purchase.State == PurchaseState.Closed)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseIsClosed));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCloseAsync(Guid purchaseId)
        {
            NotificationContext notificationContext = new();

            Domain.Entities.Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseNotFoundById));

            if (purchase.State == PurchaseState.Closed)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Purchase), ApplicationValidationMessages.PurchaseIsClosed));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
        private async Task ValidateDtoAsync(NotificationContext notificationContext, AddOrUpdatePurchaseDto purchaseDto)
        {
            ValidationResult validationResult = await purchaseDto.Validate();

            notificationContext.AddNotifications(validationResult);
        }
    }
}
