using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Product.Validations;

namespace JacksonVeroneze.StockService.Application.DTO.Product
{
    public class AddOrUpdateProductDto
    {
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateProductDtoValidator()
                .ValidateAsync(this);
    }
}
