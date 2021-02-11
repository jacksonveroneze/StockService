using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.DTO.Purchase;

namespace JacksonVeroneze.StockService.Application.DTO.PurchaseItem
{
    public class PurchaseItemDto
    {
        public int Amount { get; set; }

        public decimal Value { get; set; }

        public PurchaseDto Purchase { get; set; }

        public ProductDto Product { get; set; }
    }
}
