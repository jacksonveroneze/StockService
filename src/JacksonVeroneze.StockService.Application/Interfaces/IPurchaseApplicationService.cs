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

        Task<IEnumerable<PurchaseDto>> FindAllAsync();

        Task<ApplicationDataResult<PurchaseDto>> AddAsync(AddOrUpdatePurchaseDto data);

        Task<ApplicationDataResult<PurchaseDto>> UpdateAsync(AddOrUpdatePurchaseDto data);

        Task RemoveAsync(Guid id);

        Task Close(Guid id);

        //

        Task<PurchaseItemDto> FindItemAsync(Guid id, Guid itemId);

        Task<IEnumerable<PurchaseItemDto>> FindItensAsync(Guid id);

        Task<ApplicationDataResult<PurchaseItemDto>> AddItemAsync(Guid id, AddOrUpdatePurchaseItemDto data);

        Task<ApplicationDataResult<PurchaseItemDto>> UpdateItemAsync(Guid id, AddOrUpdatePurchaseItemDto data);

        Task RemoveItemAsync(Guid id, Guid itemId);
    }
}
