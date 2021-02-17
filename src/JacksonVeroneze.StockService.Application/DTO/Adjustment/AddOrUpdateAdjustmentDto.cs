using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Adjustment.Validations;

namespace JacksonVeroneze.StockService.Application.DTO.Adjustment
{
    public class AddOrUpdateAdjustmentDto
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateAdjustmentDtoValidator()
                .ValidateAsync(this);
    }
}
