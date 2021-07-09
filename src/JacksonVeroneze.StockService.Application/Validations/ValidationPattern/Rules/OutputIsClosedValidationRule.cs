using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Core.ValidationPattern;
using JacksonVeroneze.StockService.Domain.Enums;

namespace JacksonVeroneze.StockService.Application.Validations.ValidationPattern.Rules
{
    public class OutputIsClosedValidationRule : IValidationRule<Domain.Entities.Output>
    {
        public Notification Error => new(nameof(Output), ApplicationValidationMessages.OutputIsNotClosed);

        public bool Validate(Domain.Entities.Output value)
            => value.State == OutputState.Closed;

        public bool StopValidation { get; set; } = true;
    }
}
