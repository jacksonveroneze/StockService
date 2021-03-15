using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace JacksonVeroneze.StockService.Application.DTO.Product
{
    public class AddOrUpdateProductDto
    {
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateProductDtoValidator()
                .ValidateAsync(this);

        private class AddOrUpdateProductDtoValidator : AbstractValidator<AddOrUpdateProductDto>
        {
            public AddOrUpdateProductDtoValidator()
            {
                RuleFor(x => x.Description)
                    .Length(1, 100);
            }
        }
    }
}
