using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Application.DTO.Product;

namespace JacksonVeroneze.StockService.Application.DTO.OutputItem
{
    public class OutputItemDto
    {
        public int Amount { get; set; }

        public decimal Value { get; set; }

        public OutputDto Purchase { get; set; }

        public ProductDto Product { get; set; }
    }
}
