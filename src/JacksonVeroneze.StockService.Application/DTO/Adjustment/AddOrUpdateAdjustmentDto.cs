using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace JacksonVeroneze.StockService.Application.DTO.Adjustment
{
    public class AddOrUpdateAdjustmentDto
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateAdjustmentDtoValidator()
                .ValidateAsync(this);

        public class AddOrUpdateAdjustmentDtoValidator : AbstractValidator<AddOrUpdateAdjustmentDto>
        {
            public AddOrUpdateAdjustmentDtoValidator()
            {
                RuleFor(x => x.Description)
                    .NotEmpty()
                    .Length(1, 100);

                RuleFor(x => x.Date)
                    .NotNull();
            }
        }
    }
}
