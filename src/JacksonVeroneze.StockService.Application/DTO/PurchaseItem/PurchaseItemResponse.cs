using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.DTO.Purchase;

namespace JacksonVeroneze.StockService.Application.DTO.PurchaseItem
{
    public class PurchaseItemResponse
    {
        public int Amount { get; set; }

        public decimal Value { get; set; }

        public PurchaseResponse Purchase { get; set; }

        public ProductResultDto Product { get; set; }
    }
}
