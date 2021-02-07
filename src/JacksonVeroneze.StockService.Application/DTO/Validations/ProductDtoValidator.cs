using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(1, 100);
        }
    }
}
