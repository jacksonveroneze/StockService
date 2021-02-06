using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        Task AddASync(ProductDto productDto);

        Task UpdateASync(ProductDto productDto);

        Task RemoveASync(Guid productId);
    }
}
