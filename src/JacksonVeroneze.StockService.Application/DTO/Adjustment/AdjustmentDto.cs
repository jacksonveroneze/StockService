using System;
using JacksonVeroneze.StockService.Domain;

namespace JacksonVeroneze.StockService.Application.DTO.Adjustment
{
    public class AdjustmentDto
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalValue { get; set; }

        public AdjustmentStateEnum State { get; set; }
    }
}
