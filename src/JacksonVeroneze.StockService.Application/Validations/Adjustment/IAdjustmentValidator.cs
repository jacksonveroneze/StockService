using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Validations.Adjustment
{
    public interface IAdjustmentValidator
    {
        Task<NotificationContext> ValidateCreateAsync(AddOrUpdateAdjustmentDto adjustmentDto);

        Task<NotificationContext> ValidateUpdateAsync(Guid adjustmentId, AddOrUpdateAdjustmentDto adjustmentDto);

        Task<NotificationContext> ValidateRemoveAsync(Guid adjustmentId);

        Task<NotificationContext> ValidateCloseAsync(Guid adjustmentId);
    }
}
