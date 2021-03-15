using System;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ProductDto> FindAsync(Guid productId);

        Task<Pageable<ProductDto>> FilterAsync(Pagination pagination, ProductFilter productFilter);

        Task<ApplicationDataResult<ProductDto>> AddAsync(AddOrUpdateProductDto productDto);

        Task<ApplicationDataResult<ProductDto>> UpdateAsync(Guid productId, AddOrUpdateProductDto productDto);

        Task<ApplicationDataResult<ProductDto>> RemoveAsync(Guid productId);
    }
}
