using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Application.DTO.Product;

namespace JacksonVeroneze.StockService.Application.DTO.AdjustmentItem
{
    public class AdjustmentItemDto
    {
        public int Amount { get; set; }

        public decimal Value { get; set; }

        public AdjustmentDto Adjustment { get; set; }

        public ProductDto Product { get; set; }
    }
}
