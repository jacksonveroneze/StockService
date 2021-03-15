using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace JacksonVeroneze.StockService.Application.DTO.Output
{
    public class AddOrUpdateOutputDto
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public Task<ValidationResult> Validate()
            => new AddOrUpdateOutputDtoValidator()
                .ValidateAsync(this);

        public class AddOrUpdateOutputDtoValidator : AbstractValidator<AddOrUpdateOutputDto>
        {
            public AddOrUpdateOutputDtoValidator()
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
