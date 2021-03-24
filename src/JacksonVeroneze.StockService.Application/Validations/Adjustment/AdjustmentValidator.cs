using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.Adjustment
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class AdjustmentValidator : BaseValidator, IAdjustmentValidator
    {
        private readonly IAdjustmentRepository _adjustmentRepository;

        /// <summary>
        /// Method responsible for initialize validator.
        /// </summary>
        /// <param name="adjustmentRepository"></param>
        public AdjustmentValidator(IAdjustmentRepository adjustmentRepository)
            => _adjustmentRepository = adjustmentRepository;

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="adjustmentDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(AddOrUpdateAdjustmentDto adjustmentDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDtoAsync(notificationContext, adjustmentDto);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateUpdateAsync(Guid adjustmentId, AddOrUpdateAdjustmentDto adjustmentDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDtoAsync(notificationContext, adjustmentDto);

            Domain.Entities.Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            if (adjustment is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentNotFoundById));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateRemoveAsync(Guid adjustmentId)
        {
            NotificationContext notificationContext = new();

            Domain.Entities.Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            if (adjustment is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentNotFoundById));

            if (adjustment.HasItems)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentHasItems));

            if (adjustment.State == AdjustmentState.Closed)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentIsClosed));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCloseAsync(Guid adjustmentId)
        {
            NotificationContext notificationContext = new();

            Domain.Entities.Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            if (adjustment is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentNotFoundById));

            if (adjustment.State == AdjustmentState.Closed)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Adjustment), ApplicationValidationMessages.AdjustmentIsClosed));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="adjustmentDto"></param>
        /// <returns></returns>
        private async Task ValidateDtoAsync(NotificationContext notificationContext, AddOrUpdateAdjustmentDto adjustmentDto)
        {
            ValidationResult validationResult = await adjustmentDto.Validate();

            notificationContext.AddNotifications(validationResult);
        }
    }
}
