using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace JacksonVeroneze.StockService.Application.DTO.Purchase
{
    public class AddOrUpdatePurchaseDto
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdatePurchaseDtoValidator()
                .ValidateAsync(this);

        private class AddOrUpdatePurchaseDtoValidator : AbstractValidator<AddOrUpdatePurchaseDto>
        {
            public AddOrUpdatePurchaseDtoValidator()
            {
                RuleFor(x => x.Description)
                    .Length(1, 100);

                RuleFor(x => x.Date)
                    .NotNull()
                    .LessThan(DateTime.Now);
            }
        }
    }
}
