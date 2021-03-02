using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Util;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ProductDto> FindAsync(Guid id);

        Task<IList<ProductDto>> FilterAsync(Pagination pagination, ProductFilter filter);

        Task<ApplicationDataResult<ProductDto>> AddAsync(AddOrUpdateProductDto data);

        Task<ApplicationDataResult<ProductDto>> UpdateAsync(Guid id, AddOrUpdateProductDto data);

        Task RemoveAsync(Guid id);
    }
}
