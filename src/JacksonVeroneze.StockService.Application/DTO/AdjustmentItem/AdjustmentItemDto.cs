using System;

namespace JacksonVeroneze.StockService.Application.DTO.AdjustmentItem
{
    public class AdjustmentItemDto
    {
        public Guid Id { get; set; }

        public int Amount { get; set; }

        public decimal Value { get; set; }

        public Guid AdjustmentId { get; set; }

        public Guid ProductId { get; set; }
    }
}
