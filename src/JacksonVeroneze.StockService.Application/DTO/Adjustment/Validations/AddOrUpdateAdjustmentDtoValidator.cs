using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.Adjustment.Validations
{
    public class AddOrUpdateAdjustmentDtoValidator : AbstractValidator<AddOrUpdateAdjustmentDto>
    {
        public AddOrUpdateAdjustmentDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(x => x.Date)
                .NotNull();
        }
    }
}
