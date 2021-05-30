using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace JacksonVeroneze.StockService.Application.DTO.OutputItem
{
    public class AddOrUpdateOutputItemDto
    {
        public Guid ProductId { get; set; }

        public int Amount { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateOutputItemDtoValidator()
                .ValidateAsync(this);

        public class AddOrUpdateOutputItemDtoValidator : AbstractValidator<AddOrUpdateOutputItemDto>
        {
            public AddOrUpdateOutputItemDtoValidator()
            {
                RuleFor(x => x.Amount)
                    .NotNull()
                    .GreaterThan(0);
            }
        }
    }
}
