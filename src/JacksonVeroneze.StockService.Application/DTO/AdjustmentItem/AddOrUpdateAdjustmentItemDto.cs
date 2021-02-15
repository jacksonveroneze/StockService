using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem.Validations;

namespace JacksonVeroneze.StockService.Application.DTO.AdjustmentItem
{
    public class AddOrUpdateAdjustmentItemDto
    {
        public Guid ProductId { get; set; }

        public int Amount { get; set; }

        public decimal Value { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateAdjustmentItemDtoValidator()
                .ValidateAsync(this);
    }
}
