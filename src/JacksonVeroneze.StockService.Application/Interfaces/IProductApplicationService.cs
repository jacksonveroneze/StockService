using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO;

namespace JacksonVeroneze.StockService.Application.Interfaces
{
    public interface IProductApplicationService
    {
        public ValidationResult ValidationResult { get; }

        Task<ProductDto> AddASync(ProductDto productDto);

        Task UpdateASync(ProductDto productDto);

        Task RemoveASync(Guid productId);
    }
}
