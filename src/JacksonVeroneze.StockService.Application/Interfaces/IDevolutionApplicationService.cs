using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Devolution;
using JacksonVeroneze.StockService.Application.DTO.DevolutionItem;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IDevolutionApplicationService
    {
        Task<DevolutionDto> FindAsync(Guid devolutionId);

        Task<Pageable<DevolutionDto>> FilterAsync(Pagination pagination, DevolutionFilter filter);

        //

        Task<IList<DevolutionItemDto>> FindItensAsync(Guid outputId);
    }
}
