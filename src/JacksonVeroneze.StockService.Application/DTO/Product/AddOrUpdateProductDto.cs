using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Product.Validations;

namespace JacksonVeroneze.StockService.Application.DTO.Product
{
    public class AddOrUpdateProductDto
    {
        public string Description { get; set; }

        public Task<ValidationResult> Validate()
        {
            return new AddOrUpdateProductDtoValidator()
                .ValidateAsync(this);
        }
    }
}
