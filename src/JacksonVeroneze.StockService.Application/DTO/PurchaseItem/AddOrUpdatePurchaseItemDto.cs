using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem.Validations;

namespace JacksonVeroneze.StockService.Application.DTO.PurchaseItem
{
    public class AddOrUpdatePurchaseItemDto
    {
        public Guid ProductId { get; set; }

        public int Amount { get; set; }

        public decimal Value { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdatePurchaseItemDtoValidator()
                .ValidateAsync(this);
    }
}
