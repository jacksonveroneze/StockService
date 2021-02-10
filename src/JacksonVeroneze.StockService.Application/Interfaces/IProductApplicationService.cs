using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        Task<ProductResultDto> FindAsync(Guid id);

        Task<IEnumerable<ProductResultDto>> FindAllAsync();

        Task<ApplicationDataResult<ProductResultDto>> AddASync(ProductRequestDto productRequestDto);

        Task<ApplicationDataResult<ProductResultDto>> UpdateASync(ProductRequestDto productRequestDto);

        Task RemoveASync(Guid id);
    }
}
