using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.AdjustmentItem
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class AdjustmentItemValidator : BaseValidator, IAdjustmentItemValidator
    {
        private readonly IProductRepository _productRepository;
        private readonly IAdjustmentRepository _adjustmentRepository;

        /// <summary>
        /// Method responsible for initialize validator.
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="productRepository1"></param>
        public AdjustmentItemValidator(IAdjustmentRepository productRepository, IProductRepository productRepository1)
        {
            _adjustmentRepository = productRepository;
            _productRepository = productRepository1;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentItemDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(Guid adjustmentId, AddOrUpdateAdjustmentItemDto adjustmentItemDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDefaultActionsAsync(notificationContext, adjustmentId, default);
            await ValidateDtoAsync(notificationContext, adjustmentItemDto);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentItemId"></param>
        /// <param name="adjustmentItemDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateUpdateAsync(Guid adjustmentId, Guid adjustmentItemId,
            AddOrUpdateAdjustmentItemDto adjustmentItemDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDefaultActionsAsync(notificationContext, adjustmentId, adjustmentItemId);
            await ValidateDtoAsync(notificationContext, adjustmentItemDto);

            return notificationContext;
        }

        /// <summary>
        ///  Method responsible for validation.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentItemId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateRemoveAsync(Guid adjustmentId, Guid adjustmentItemId)
        {
            NotificationContext notificationContext = new();

            await ValidateDefaultActionsAsync(notificationContext, adjustmentId, adjustmentItemId);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="adjustmentItemDto"></param>
        /// <returns></returns>
        private async Task ValidateDtoAsync(NotificationContext notificationContext,
            AddOrUpdateAdjustmentItemDto adjustmentItemDto)
        {
            ValidationResult validationResult = await adjustmentItemDto.Validate();

            notificationContext.AddNotifications(validationResult);

            Domain.Entities.Product product = await _productRepository.FindAsync(adjustmentItemDto.ProductId);

            if (product is null)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.ProductNotFoundById));
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentItemId"></param>
        /// <returns></returns>
        private async Task ValidateDefaultActionsAsync(NotificationContext notificationContext, Guid adjustmentId,
            Guid? adjustmentItemId)
        {
            Domain.Entities.Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            if (adjustment is null)
            {
                notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentNotFoundById));

                return;
            }

            if (adjustment.State == AdjustmentState.Closed)
            {
                notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentIsClosed));

                return;
            }

            if (adjustmentItemId.HasValue is false) return;

            Domain.Entities.AdjustmentItem adjustmentItem = adjustment.FindItem(adjustmentItemId.Value);

            if (adjustmentItem is null)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentItemNotFoundById));
        }
    }
}
