using System;
using System.Linq;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Core.ValidationPattern;

namespace JacksonVeroneze.StockService.Application.Validations.ValidationPattern.Rules
{
    public class OutputItemWithMovementValidationRule : IValidationRule<Domain.Entities.Output>
    {
        private readonly Guid _outputId;

        public OutputItemWithMovementValidationRule(Guid outputId) => _outputId = outputId;

        public Notification Error =>
            new(nameof(Output), ApplicationValidationMessages.OutputItemWithoutMovementItem);

        public bool Validate(Domain.Entities.Output value)
            => value.FindItem(_outputId).MovementItems.Any();

        public bool StopValidation { get; set; } = true;
    }
}
