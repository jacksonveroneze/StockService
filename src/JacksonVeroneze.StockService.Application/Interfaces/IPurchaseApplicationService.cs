using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IPurchaseApplicationService
    {
        Task<PurchaseDto> FindAsync(Guid id);

        Task<IList<PurchaseDto>> FindAllAsync();

        Task<ApplicationDataResult<PurchaseDto>> AddAsync(AddOrUpdatePurchaseDto data);

        Task<ApplicationDataResult<PurchaseDto>> UpdateAsync(Guid id, AddOrUpdatePurchaseDto data);

        Task RemoveAsync(Guid id);

        Task CloseAsync(Guid id);

        //

        Task<PurchaseItemDto> FindItemAsync(Guid id, Guid itemId);

        Task<IList<PurchaseItemDto>> FindItensAsync(Guid id);

        Task<ApplicationDataResult<PurchaseItemDto>> AddItemAsync(AddOrUpdatePurchaseItemDto data);

        Task<ApplicationDataResult<PurchaseItemDto>> UpdateItemAsync(AddOrUpdatePurchaseItemDto data);

        Task RemoveItemAsync(Guid id, Guid itemId);
    }
}
