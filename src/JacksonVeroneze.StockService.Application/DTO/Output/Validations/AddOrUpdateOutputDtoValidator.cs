using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.Output.Validations
{
    public class AddOrUpdateOutputDtoValidator : AbstractValidator<AddOrUpdateOutputDto>
    {
        public AddOrUpdateOutputDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(x => x.Date)
                .NotNull();
        }
    }
}
