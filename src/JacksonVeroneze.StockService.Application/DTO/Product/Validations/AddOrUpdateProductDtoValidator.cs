using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.Product.Validations
{
    public class AddOrUpdateProductDtoValidator : AbstractValidator<AddOrUpdateProductDto>
    {
        public AddOrUpdateProductDtoValidator()
        {
            RuleFor(x => x.Description)
                .Length(1, 100);
        }
    }
}
