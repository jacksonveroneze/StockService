using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.OutputItem.Validations;

namespace JacksonVeroneze.StockService.Application.DTO.OutputItem
{
    public class AddOrUpdateOutputItemDto
    {
        public Guid ProductId { get; set; }

        public int Amount { get; set; }

        public decimal Value { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateOutputItemDtoValidator()
                .ValidateAsync(this);
    }
}
