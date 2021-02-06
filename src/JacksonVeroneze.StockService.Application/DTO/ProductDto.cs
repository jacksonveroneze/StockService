using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO
{
    public class ProductDto
    {
        public string Description { get; set; }
    }

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
