using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.Product.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductRequestDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(1, 100);
        }
    }
}
