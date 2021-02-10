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
        Task<PurchaseResponse> FindAsync(Guid id);

        Task<IEnumerable<PurchaseResponse>> FindAllAsync();

        Task<ApplicationDataResult<PurchaseResponse>> AddAsync(PurchaseAddUpdateRequest updateRequest);

        Task<ApplicationDataResult<PurchaseResponse>> UpdateAsync(PurchaseAddUpdateRequest updateRequest);

        Task RemoveAsync(Guid id);

        Task Close(Guid id);

        //

        Task<PurchaseItemResponse> FindItemAsync(Guid id, Guid itemId);

        Task<IEnumerable<PurchaseItemResponse>> FindItensAsync(Guid id);

        Task<ApplicationDataResult<PurchaseResponse>> AddItemAsync(Guid id, PurchaseAddUpdateItem request);

        Task<ApplicationDataResult<PurchaseResponse>> UpdateItemAsync(Guid id, PurchaseAddUpdateItem request);

        Task RemoveItemAsync(Guid id, Guid itemId);
    }
}
