using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.OutputItem.Validations
{
    public class AddOrUpdateOutputItemDtoValidator : AbstractValidator<AddOrUpdateOutputItemDto>
    {
        public AddOrUpdateOutputItemDtoValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Value)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.ProductId)
                .NotNull();
        }
    }
}
