using System;
using JacksonVeroneze.StockService.Domain.Enums;

namespace JacksonVeroneze.StockService.Application.DTO.Output
{
    public class OutputDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalValue { get; set; }

        public OutputState State { get; set; }
    }
}
