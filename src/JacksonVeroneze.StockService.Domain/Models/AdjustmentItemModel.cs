using System;

namespace JacksonVeroneze.StockService.Domain.Models
{
    public class AdjustmentItemModel
    {
        public Guid Id { get; set; }

        public int Amount { get; set; }

        public Guid AdjustmentId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductDescription { get; set; }
    }
}
