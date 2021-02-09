using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ProductDto> FindAsync(Guid id);

        Task<IEnumerable<ProductDto>> FindAllAsync();

        Task<ApplicationDataResult<ProductDto>> AddASync(ProductDto productDto);

        Task<ApplicationDataResult<ProductDto>> UpdateASync(ProductDto productDto);

        Task RemoveASync(Guid id);
    }
}
