using System;
using FluentValidation;

namespace JacksonVeroneze.StockService.Application.DTO.Purchase.Validations
{
    public class AddOrUpdatePurchaseDtoValidator : AbstractValidator<AddOrUpdatePurchaseDto>
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
