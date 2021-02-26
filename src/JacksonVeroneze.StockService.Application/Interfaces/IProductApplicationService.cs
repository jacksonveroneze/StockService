using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ProductDto> FindAsync(Guid id);

        Task<IList<ProductDto>> FilterAsync(Pagination pagination, ProductFilter filter);

        Task<ApplicationDataResult<ProductDto>> AddASync(AddOrUpdateProductDto data);

        Task<ApplicationDataResult<ProductDto>> UpdateASync(Guid id, AddOrUpdateProductDto data);

        Task RemoveASync(Guid id);
    }
}
