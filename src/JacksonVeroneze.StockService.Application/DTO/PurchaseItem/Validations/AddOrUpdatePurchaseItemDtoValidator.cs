using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.PurchaseItem.Validations
{
    public class AddOrUpdatePurchaseItemDtoValidator : AbstractValidator<AddOrUpdatePurchaseItemDto>
    {
        public AddOrUpdatePurchaseItemDtoValidator()
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
