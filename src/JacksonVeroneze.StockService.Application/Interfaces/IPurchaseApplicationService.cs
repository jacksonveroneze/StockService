using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IPurchaseApplicationService
    {
        Task<PurchaseDto> FindAsync(Guid purchaseId);

        Task<IList<PurchaseDto>> FilterAsync(Pagination pagination, PurchaseFilter filter);

        Task<ApplicationDataResult<PurchaseDto>> AddAsync(AddOrUpdatePurchaseDto purchaseDto);

        Task<ApplicationDataResult<PurchaseDto>> UpdateAsync(Guid purchaseId, AddOrUpdatePurchaseDto purchaseDto);

        Task<ApplicationDataResult<PurchaseDto>> RemoveAsync(Guid purchaseId);

        Task<ApplicationDataResult<PurchaseDto>> CloseAsync(Guid purchaseId);

        //

        Task<PurchaseItemDto> FindItemAsync(Guid purchaseId, Guid purchaseItemId);

        Task<IList<PurchaseItemDto>> FindItensAsync(Guid purchaseId);

        Task<ApplicationDataResult<PurchaseItemDto>> AddItemAsync(Guid purchaseId,
            AddOrUpdatePurchaseItemDto purchaseItemDto);

        Task<ApplicationDataResult<PurchaseItemDto>> UpdateItemAsync(Guid purchaseId, Guid purchaseItemId,
            AddOrUpdatePurchaseItemDto purchaseItemDto);

        Task<ApplicationDataResult<PurchaseItemDto>> RemoveItemAsync(Guid purchaseId, Guid purchaseItemId);
    }
}
