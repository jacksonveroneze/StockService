using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IPurchaseApplicationService
    {
        Task<PurchaseDto> FindAsync(Guid id);

        Task<IList<PurchaseDto>> FilterAsync(Pagination pagination, PurchaseFilter filter);

        Task<ApplicationDataResult<PurchaseDto>> AddAsync(AddOrUpdatePurchaseDto data);

        Task<ApplicationDataResult<PurchaseDto>> UpdateAsync(Guid id, AddOrUpdatePurchaseDto data);

        Task RemoveAsync(Guid id);

        Task CloseAsync(Guid id);

        //

        Task<PurchaseItemDto> FindItemAsync(Guid id, Guid itemId);

        Task<IList<PurchaseItemDto>> FindItensAsync(Guid id);

        Task<ApplicationDataResult<PurchaseItemDto>> AddItemAsync(Guid id, AddOrUpdatePurchaseItemDto data);

        Task<ApplicationDataResult<PurchaseItemDto>> UpdateItemAsync(Guid id, Guid idItem, AddOrUpdatePurchaseItemDto data);

        Task RemoveItemAsync(Guid id, Guid itemId);
    }
}
