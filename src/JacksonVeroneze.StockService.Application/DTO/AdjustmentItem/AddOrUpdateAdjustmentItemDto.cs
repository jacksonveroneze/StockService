using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

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

        public class AddOrUpdateAdjustmentItemDtoValidator : AbstractValidator<AddOrUpdateAdjustmentItemDto>
        {
            public AddOrUpdateAdjustmentItemDtoValidator()
            {
                RuleFor(x => x.Amount)
                    .NotNull()
                    .GreaterThan(0);

                RuleFor(x => x.Value)
                    .NotNull()
                    .GreaterThan(0);

                RuleFor(x => x.ProductId)
                    .NotNull();
            }
        }
    }
}
