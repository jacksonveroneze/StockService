using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.AdjustmentItem.Validations
{
    public class AddOrUpdateAdjustmentItemDtoValidator : AbstractValidator<AddOrUpdateAdjustmentItemDto>
    {
        public AddOrUpdateAdjustmentItemDtoValidator()
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
