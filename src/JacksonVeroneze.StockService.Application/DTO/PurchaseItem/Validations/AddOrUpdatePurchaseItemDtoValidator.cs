using FluentValidation;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.DTO.PurchaseItem.Validations
{
    public class AddOrUpdatePurchaseItemDtoValidator : AbstractValidator<AddOrUpdatePurchaseItemDto>
    {
        public AddOrUpdatePurchaseItemDtoValidator(IPurchaseRepository purchaseRepository, IProductRepository productRepository)
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Value)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.PurchaseId)
                .NotNull()
                .MustAsync(async (request, val, token) =>
                {
                    Domain.Entities.Purchase product = await purchaseRepository.FindAsync(val);

                    return product != null;
                }).WithMessage("O registro pai informado está incorreto.");

            RuleFor(x => x.ProductId)
                .NotNull()
                .MustAsync(async (request, val, token) =>
                {
                    Domain.Entities.Product product = await productRepository.FindAsync(val);

                    return product != null;
                }).WithMessage("O produto informado está incorreto.");
        }
    }
}
