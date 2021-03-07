using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.Purchase
{
    public class PurchaseValidator : IPurchaseValidator
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseValidator(IPurchaseRepository productRepository)
            => _purchaseRepository = productRepository;

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(AddOrUpdatePurchaseDto purchaseDto)
        {
            NotificationContext notificationContext = new();

            await ValidatePurchaseDto(notificationContext, purchaseDto);

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

            await ValidateIfExistPurchase(notificationContext, purchaseId);
            await ValidateIfIsClose(notificationContext, purchaseId);
            await ValidatePurchaseDto(notificationContext, purchaseDto);

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

            await ValidateIfExistPurchase(notificationContext, purchaseId);
            await ValidateIfIsClose(notificationContext, purchaseId);

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

            await ValidateIfExistPurchase(notificationContext, purchaseId);
            await ValidateIfIsClose(notificationContext, purchaseId);

            return notificationContext;
        }

        private async Task ValidateIfExistPurchase(NotificationContext notificationContext, Guid purchaseId)
        {
            Domain.Entities.Purchase product = await _purchaseRepository.FindAsync(purchaseId);

            if (product is null)
                notificationContext.AddNotification(new Notification("product", "Compra näo encontrado"));
        }

        private async Task ValidatePurchaseDto(NotificationContext notificationContext,
            AddOrUpdatePurchaseDto purchaseDto)
        {
            ValidationResult validationResult = await purchaseDto.Validate();

            notificationContext.AddNotifications(validationResult);
        }

        private async Task ValidateIfIsClose(NotificationContext notificationContext, Guid purchaseId)
        {
            Domain.Entities.Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase.State == PurchaseState.Closed)
                notificationContext.AddNotification(new Notification("purchase",
                    "A compra informada já encontra-se fechada, portanto não pode ser movimentada ou excluida."));
        }
    }
}
