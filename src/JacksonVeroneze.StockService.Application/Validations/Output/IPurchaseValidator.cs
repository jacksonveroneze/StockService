using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Validations.Output
{
    public interface IOutputValidator
    {
        Task<NotificationContext> ValidateCreateAsync(AddOrUpdateOutputDto outputDto);

        Task<NotificationContext> ValidateUpdateAsync(Guid outputId, AddOrUpdateOutputDto outputDto);

        Task<NotificationContext> ValidateRemoveAsync(Guid outputId);

        Task<NotificationContext> ValidateCloseAsync(Guid outputId);
    }
}
