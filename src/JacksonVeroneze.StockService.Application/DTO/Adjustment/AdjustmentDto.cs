using System;
using JacksonVeroneze.StockService.Domain;
using JacksonVeroneze.StockService.Domain.Enums;

namespace JacksonVeroneze.StockService.Application.DTO.Adjustment
{
    public class AdjustmentDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalValue { get; set; }

        public AdjustmentState State { get; set; }
    }
}
