using System;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Core.ValidationPattern;

namespace JacksonVeroneze.StockService.Application.Validations.ValidationPattern.Rules
{
    public class OutputItemExistsValidationRule : IValidationRule<Domain.Entities.Output>
    {
        private readonly Guid _outputId;

        public OutputItemExistsValidationRule(Guid outputId) => _outputId = outputId;
        public Notification Error => new(nameof(Output), ApplicationValidationMessages.OutputItemNotFoundById);

        public bool Validate(Domain.Entities.Output value)
            => value.CheckIfExistsItemById(_outputId);

        public bool StopValidation { get; set; } = true;
    }
}
