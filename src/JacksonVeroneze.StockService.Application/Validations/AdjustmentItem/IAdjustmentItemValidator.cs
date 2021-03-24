using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Validations.AdjustmentItem
{
    public interface IAdjustmentItemValidator
    {
        Task<NotificationContext> ValidateCreateAsync(Guid adjustmentId, AddOrUpdateAdjustmentItemDto adjustmentItemDto);

        Task<NotificationContext> ValidateUpdateAsync(Guid adjustmentId, Guid adjustmentItemId,
            AddOrUpdateAdjustmentItemDto adjustmentItemDto);

        Task<NotificationContext> ValidateRemoveAsync(Guid adjustmentId, Guid adjustmentItemId);
    }
}
