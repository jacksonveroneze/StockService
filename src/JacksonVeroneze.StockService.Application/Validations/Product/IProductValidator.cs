using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Core;

namespace JacksonVeroneze.StockService.Application.Validations.Product
{
    public interface IProductValidator
    {
        Task<NotificationContext> Validate(AddOrUpdateProductDto productDto);

        Task<NotificationContext> Validate(Guid productId);

        Task<NotificationContext> Validate(Guid productId, AddOrUpdateProductDto productDto);
    }
}
