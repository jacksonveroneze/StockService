using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Validations.Purchase
{
    public interface IPurchaseValidator
    {
        Task<NotificationContext> ValidateCreateAsync(AddOrUpdatePurchaseDto purchaseDto);

        Task<NotificationContext> ValidateUpdateAsync(Guid purchaseId, AddOrUpdatePurchaseDto purchaseDto);

        Task<NotificationContext> ValidateRemoveAsync(Guid purchaseId);

        Task<NotificationContext> ValidateCloseAsync(Guid purchaseId);
    }
}
