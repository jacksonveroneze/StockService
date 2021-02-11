using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Purchase.Validations;

namespace JacksonVeroneze.StockService.Application.DTO.Purchase
{
    public class AddOrUpdatePurchaseDto
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdatePurchaseDtoValidator()
                .ValidateAsync(this);
    }
}
