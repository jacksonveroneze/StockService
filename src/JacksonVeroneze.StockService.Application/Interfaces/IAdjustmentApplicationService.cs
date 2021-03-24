using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IAdjustmentApplicationService
    {
        Task<AdjustmentDto> FindAsync(Guid adjustmentId);

        Task<Pageable<AdjustmentDto>> FilterAsync(Pagination pagination, AdjustmentFilter filter);

        Task<ApplicationDataResult<AdjustmentDto>> AddAsync(AddOrUpdateAdjustmentDto adjustmentDto);

        Task<ApplicationDataResult<AdjustmentDto>> UpdateAsync(Guid adjustmentId, AddOrUpdateAdjustmentDto adjustmentDto);

        Task<ApplicationDataResult<AdjustmentDto>> RemoveAsync(Guid adjustmentId);

        Task<ApplicationDataResult<AdjustmentDto>> CloseAsync(Guid adjustmentId);

        //

        Task<AdjustmentItemDto> FindItemAsync(Guid adjustmentId, Guid adjustmentItemId);

        Task<IList<AdjustmentItemDto>> FindItensAsync(Guid adjustmentId);

        Task<ApplicationDataResult<AdjustmentItemDto>> AddItemAsync(Guid adjustmentId,
            AddOrUpdateAdjustmentItemDto adjustmentItemDto);

        Task<ApplicationDataResult<AdjustmentItemDto>> UpdateItemAsync(Guid adjustmentId, Guid adjustmentItemId,
            AddOrUpdateAdjustmentItemDto adjustmentItemDto);

        Task<ApplicationDataResult<AdjustmentItemDto>> RemoveItemAsync(Guid adjustmentId, Guid adjustmentItemId);
    }
}
