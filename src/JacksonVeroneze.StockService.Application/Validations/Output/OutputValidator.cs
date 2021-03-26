using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Enums;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Validations.Output
{
    /// <summary>
    /// Class responsible for validator.
    /// </summary>
    public class OutputValidator : BaseValidator, IOutputValidator
    {
        private readonly IOutputRepository _outputRepository;
        private readonly IMovementRepository _movementRepository;

        /// <summary>
        /// Method responsible for initialize validator.
        /// </summary>
        /// <param name="outputRepository"></param>
        /// <param name="movementRepository"></param>
        public OutputValidator(IOutputRepository outputRepository, IMovementRepository movementRepository)
        {
            _outputRepository = outputRepository;
            _movementRepository = movementRepository;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="outputDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCreateAsync(AddOrUpdateOutputDto outputDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDtoAsync(notificationContext, outputDto);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputDto"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateUpdateAsync(Guid outputId, AddOrUpdateOutputDto outputDto)
        {
            NotificationContext notificationContext = new();

            await ValidateDtoAsync(notificationContext, outputDto);

            Domain.Entities.Output output = await _outputRepository.FindAsync(outputId);

            if (output is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputNotFoundById));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="outputId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateRemoveAsync(Guid outputId)
        {
            NotificationContext notificationContext = new();

            Domain.Entities.Output output = await _outputRepository.FindAsync(outputId);

            if (output is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputNotFoundById));

            if (output.HasItems)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputHasItems));

            if (output.State == OutputState.Closed)
                notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputIsClosed));

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="outputId"></param>
        /// <returns></returns>
        public async Task<NotificationContext> ValidateCloseAsync(Guid outputId)
        {
            NotificationContext notificationContext = new();

            Domain.Entities.Output output = await _outputRepository.FindAsync(outputId);

            if (output is null)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputNotFoundById));

            if (output.State == OutputState.Closed)
                return notificationContext.AddNotification(
                    CreateNotification(nameof(Output), ApplicationValidationMessages.OutputIsClosed));

            await ValidateHasStockAsync(notificationContext, output);

            return notificationContext;
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="outputDto"></param>
        /// <returns></returns>
        private async Task ValidateDtoAsync(NotificationContext notificationContext, AddOrUpdateOutputDto outputDto)
        {
            ValidationResult validationResult = await outputDto.Validate();

            notificationContext.AddNotifications(validationResult);
        }

        /// <summary>
        /// Method responsible for validation.
        /// </summary>
        /// <param name="notificationContext"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private async Task ValidateHasStockAsync(NotificationContext notificationContext, Domain.Entities.Output output)
        {
            IList<Guid> listProducts = output.Items.Select(x => x.Product.Id).ToList();

            IList<Movement> listMovements =
                await _movementRepository.FilterAsync(new MovementFilter {ProductIds = listProducts});

            foreach (Domain.Entities.OutputItem outputItem in output.Items)
            {
                Movement movement = listMovements.FirstOrDefault(x => x.Product.Id == outputItem.Product.Id);

                if (movement is null)
                {
                    notificationContext.AddNotification(
                        CreateNotification(nameof(Output), ApplicationValidationMessages.OutputItemProductNotMovement));
                }
                else
                {
                    int? lastAmmount = movement.FindLastAmmount();

                    if (lastAmmount.HasValue is false || lastAmmount.Value < outputItem.Amount)
                        notificationContext.AddNotification(CreateNotification(nameof(Output),
                            ApplicationValidationMessages.OutputItemProductNotSufficientStock));
                }
            }
        }
    }
}
