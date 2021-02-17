using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IOutputApplicationService
    {
        Task<OutputDto> FindAsync(Guid id);

        Task<IEnumerable<OutputDto>> FindAllAsync();

        Task<ApplicationDataResult<OutputDto>> AddAsync(AddOrUpdateOutputDto data);

        Task<ApplicationDataResult<OutputDto>> UpdateAsync(AddOrUpdateOutputDto data);

        Task RemoveAsync(Guid id);

        Task Close(Guid id);

        //

        Task<OutputItemDto> FindItemAsync(Guid id, Guid itemId);

        Task<IEnumerable<OutputItemDto>> FindItensAsync(Guid id);

        Task<ApplicationDataResult<OutputItemDto>> AddItemAsync(Guid id, AddOrUpdateOutputItemDto data);

        Task<ApplicationDataResult<OutputItemDto>> UpdateItemAsync(Guid id, AddOrUpdateOutputItemDto data);

        Task RemoveItemAsync(Guid id, Guid itemId);
    }
}
