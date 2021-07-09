using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Core.ValidationPattern;

namespace JacksonVeroneze.StockService.Application.Validations.ValidationPattern.Rules
{
    public class OutputExistsValidationRule : IValidationRule<Domain.Entities.Output>
    {
        public Notification Error =>
            new(nameof(Output), ApplicationValidationMessages.OutputNotFoundById);

        public bool Validate(Domain.Entities.Output value)
            => value != null;

        public bool StopValidation { get; set; } = true;
    }
}
