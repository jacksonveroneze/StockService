using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

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

        public class AddOrUpdatePurchaseItemDtoValidator : AbstractValidator<AddOrUpdatePurchaseItemDto>
        {
            public AddOrUpdatePurchaseItemDtoValidator()
            {
                RuleFor(x => x.Amount)
                    .NotNull()
                    .GreaterThan(0);

                RuleFor(x => x.Value)
                    .NotNull()
                    .GreaterThan(0);
            }
        }
    }
}
