using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Validations.PurchaseItem
{
    public interface IPurchaseItemValidator
    {
        Task<NotificationContext> ValidateCreateAsync(Guid purchaseId, AddOrUpdatePurchaseItemDto purchaseItemDto);

        Task<NotificationContext> ValidateUpdateAsync(Guid purchaseId, Guid purchaseItemId,
            AddOrUpdatePurchaseItemDto purchaseItemDto);

        Task<NotificationContext> ValidateRemoveAsync(Guid purchaseId, Guid purchaseItemId);
    }
}
