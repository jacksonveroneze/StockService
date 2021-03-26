using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.OutputItem
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class OutputItemValidator : BaseValidator, IOutputItemValidator
    {
        private readonly IOutputRepository _outputRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMovementRepository _movementRepository;

        /// <summary>
        /// Method responsible for initialize validator.
        /// </summary>
        /// <param name="outputRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="movementRepository"></param>
        public OutputItemValidator(IOutputRepository outputRepository, IProductRepository productRepository,
            IMovementRepository movementRepository)
        {
            _outputRepository = outputRepository;
            _productRepository = productRepository;
            _movementRepository = movementRepository;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(Guid outputId,
            AddOrUpdateOutputItemDto outputItemDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDefaultActionsAsync(notificationContext, outputId, default);
            await ValidateDtoAsync(notificationContext, outputItemDto);
            await ValidateHasStockAsync(notificationContext, outputItemDto);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <param name="outputItemDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateUpdateAsync(Guid outputId, Guid outputItemId,
            AddOrUpdateOutputItemDto outputItemDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDefaultActionsAsync(notificationContext, outputId, outputItemId);
            await ValidateDtoAsync(notificationContext, outputItemDto);
            await ValidateHasStockAsync(notificationContext, outputItemDto);

            return notificationContext;
        }

        /// <summary>
        ///  Method responsible for validation.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateRemoveAsync(Guid outputId, Guid outputItemId)
        {
            NotificationContext notificationContext = new();

            await ValidateDefaultActionsAsync(notificationContext, outputId, outputItemId);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="outputItemDto"></param>
        /// <returns></returns>
        private async Task ValidateDtoAsync(NotificationContext notificationContext,
            AddOrUpdateOutputItemDto outputItemDto)
        {
            ValidationResult validationResult = await outputItemDto.Validate();

            notificationContext.AddNotifications(validationResult);

            Domain.Entities.Product product = await _productRepository.FindAsync(outputItemDto.ProductId);

            if (product is null)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.ProductNotFoundById));
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <returns></returns>
        private async Task ValidateDefaultActionsAsync(NotificationContext notificationContext, Guid outputId,
            Guid? outputItemId)
        {
            Domain.Entities.Output output = await _outputRepository.FindAsync(outputId);

            if (output is null)
            {
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputNotFoundById));

                return;
            }

            if (output.State == OutputState.Closed)
            {
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputIsClosed));

                return;
            }

            if (outputItemId.HasValue is false) return;

            Domain.Entities.OutputItem outputItem = output.FindItem(outputItemId.Value);

            if (outputItem is null)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputItemNotFoundById));
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="outputItemDto"></param>
        /// <returns></returns>
        private async Task ValidateHasStockAsync(NotificationContext notificationContext,
            AddOrUpdateOutputItemDto outputItemDto)
        {
            Movement movement =
                await _movementRepository.FindAsync(new MovementFilter {ProductId = outputItemDto.ProductId});

            if (movement is null)
            {
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputItemProductNotMovement));

                return;
            }

            int? lastAmmount = movement.FindLastAmmount();

            if (lastAmmount.HasValue is false || lastAmmount.Value < outputItemDto.Amount)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputItemProductNotSufficientStock));
        }
    }
}
