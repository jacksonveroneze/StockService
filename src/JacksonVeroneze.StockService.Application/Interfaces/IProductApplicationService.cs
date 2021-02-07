using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ApplicationDataResult<ProductDto>> AddASync(ProductDto productDto);

        Task UpdateASync(ProductDto productDto);

        Task RemoveASync(Guid productId);
    }
}
