using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IAdjustmentApplicationService
    {
        Task<AdjustmentDto> FindAsync(Guid id);

        Task<IEnumerable<AdjustmentDto>> FindAllAsync();

        Task<ApplicationDataResult<AdjustmentDto>> AddAsync(AddOrUpdateAdjustmentDto data);

        Task<ApplicationDataResult<AdjustmentDto>> UpdateAsync(AddOrUpdateAdjustmentDto data);

        Task RemoveAsync(Guid id);

        Task Close(Guid id);

        //

        Task<AdjustmentItemDto> FindItemAsync(Guid id, Guid itemId);

        Task<IEnumerable<AdjustmentItemDto>> FindItensAsync(Guid id);

        Task<ApplicationDataResult<AdjustmentItemDto>> AddItemAsync(Guid id, AddOrUpdateAdjustmentItemDto data);

        Task<ApplicationDataResult<AdjustmentItemDto>> UpdateItemAsync(Guid id, AddOrUpdateAdjustmentItemDto data);

        Task RemoveItemAsync(Guid id, Guid itemId);
    }
}
