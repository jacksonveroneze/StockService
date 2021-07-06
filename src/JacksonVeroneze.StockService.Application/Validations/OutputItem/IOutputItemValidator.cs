using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Validations.OutputItem
{
    public interface IOutputItemValidator
    {
        Task<NotificationContext> ValidateCreateAsync(Guid outputId, AddOrUpdateOutputItemDto outputItemDto);

        Task<NotificationContext> ValidateUpdateAsync(Guid outputId, Guid outputItemId,
            AddOrUpdateOutputItemDto outputItemDto);

        Task<NotificationContext> ValidateRemoveAsync(Guid outputId, Guid outputItemId);
        
        Task<NotificationContext> ValidateUndoItemAsync(Guid outputId, Guid outputItemId);
    }
}
