using System;
using System.Linq;
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

        private Domain.Entities.Output _output;

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

            await ValidateExistsOutputAsync(notificationContext, outputId);
            await ValidateOutputIsClosedAsync(notificationContext, outputId);
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

            await ValidateExistsOutputAsync(notificationContext, outputId);
            await ValidateOutputIsClosedAsync(notificationContext, outputId);
            await ValidateExistsOutputItemAsync(notificationContext, outputId, outputItemId);
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

            await ValidateExistsOutputAsync(notificationContext, outputId);
            await ValidateOutputIsClosedAsync(notificationContext, outputId);
            await ValidateExistsOutputItemAsync(notificationContext, outputId, outputItemId);

            return notificationContext;
        }

        /// <summary>
        ///  Method responsible for validation.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateUndoItemAsync(Guid outputId, Guid outputItemId)
        {
            NotificationContext notificationContext = new();

            await ValidateExistsOutputAsync(notificationContext, outputId);

            if (notificationContext.HasNotifications)
                return notificationContext;

            await ValidateOutputIsOpenAsync(notificationContext, outputId);
            await ValidateExistsOutputItemAsync(notificationContext, outputId, outputItemId);

            if (notificationContext.HasNotifications)
                return notificationContext;

            Domain.Entities.OutputItem outputItem = _output.FindItem(outputItemId);

            if (outputItem.MovementItems.Any() is false)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), "Item de saída não gerou movimentação."));

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
        /// <returns></returns>
        private async Task ValidateExistsOutputAsync(NotificationContext notificationContext, Guid outputId)
        {
            Domain.Entities.Output output = await FindOutput(outputId);

            if (output is null)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputNotFoundById));
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="outputId"></param>
        /// <returns></returns>
        private async Task ValidateOutputIsOpenAsync(NotificationContext notificationContext, Guid outputId)
        {
            Domain.Entities.Output output = await FindOutput(outputId);

            if (output.State == OutputState.Open)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputIsOpened));
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="outputId"></param>
        /// <returns></returns>
        private async Task ValidateOutputIsClosedAsync(NotificationContext notificationContext, Guid outputId)
        {
            Domain.Entities.Output output = await FindOutput(outputId);

            if (output.State == OutputState.Closed)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputIsClosed));
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <returns></returns>
        private async Task ValidateExistsOutputItemAsync(NotificationContext notificationContext, Guid outputId,
            Guid outputItemId)
        {
            Domain.Entities.Output output = await FindOutput(outputId);

            Domain.Entities.OutputItem outputItem = output.FindItem(outputItemId);

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
                    CreateNotification(nameof(Output),
                        ApplicationValidationMessages.OutputItemProductNotSufficientStock));
        }

        private async Task<Domain.Entities.Output> FindOutput(Guid outputId)
            => _output ??= await _outputRepository.FindAsync(outputId);
    }
}
