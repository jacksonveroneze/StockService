using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Output.Validations;

namespace JacksonVeroneze.StockService.Application.DTO.Output
{
    public class AddOrUpdateOutputDto
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateOutputDtoValidator()
                .ValidateAsync(this);
    }
}
