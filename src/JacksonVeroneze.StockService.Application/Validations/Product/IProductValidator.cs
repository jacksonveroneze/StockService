using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Application.Validations.Product
{
    public interface IProductValidator
    {
        Task<NotificationContext> ValidateCreateAsync(AddOrUpdateProductDto productDto);

        Task<NotificationContext> ValidateUpdateAsync(Guid productId, AddOrUpdateProductDto productDto);

        Task<NotificationContext> ValidateRemoveAsync(Guid productId);
    }
}
