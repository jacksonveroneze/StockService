using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ProductDto> FindAsync(Guid id);

        Task<IEnumerable<ProductDto>> FindAllAsync();

        Task<ApplicationDataResult<ProductDto>> AddASync(AddOrUpdateProductDto data);

        Task<ApplicationDataResult<ProductDto>> UpdateASync(AddOrUpdateProductDto data);

        Task RemoveASync(Guid id);
    }
}
