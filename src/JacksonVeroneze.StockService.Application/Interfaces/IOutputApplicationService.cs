using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IOutputApplicationService
    {
        Task<OutputDto> FindAsync(Guid outputId);

        Task<Pageable<OutputDto>> FilterAsync(Pagination pagination, OutputFilter filter);

        Task<ApplicationDataResult<OutputDto>> AddAsync(AddOrUpdateOutputDto outputDto);

        Task<ApplicationDataResult<OutputDto>> UpdateAsync(Guid outputId, AddOrUpdateOutputDto outputDto);

        Task<ApplicationDataResult<OutputDto>> RemoveAsync(Guid outputId);

        Task<ApplicationDataResult<OutputDto>> CloseAsync(Guid outputId);

        //

        Task<OutputItemDto> FindItemAsync(Guid outputId, Guid outputItemId);

        Task<IList<OutputItemDto>> FindItensAsync(Guid outputId);

        Task<ApplicationDataResult<OutputItemDto>> AddItemAsync(Guid outputId,
            AddOrUpdateOutputItemDto outputItemDto);

        Task<ApplicationDataResult<OutputItemDto>> UpdateItemAsync(Guid outputId, Guid outputItemId,
            AddOrUpdateOutputItemDto outputItemDto);

        Task<ApplicationDataResult<OutputItemDto>> RemoveItemAsync(Guid outputId, Guid outputItemId);

        Task<ApplicationDataResult<OutputItemDto>> UndoItemAsync(Guid outputId, Guid outputItemId);
    }
}
