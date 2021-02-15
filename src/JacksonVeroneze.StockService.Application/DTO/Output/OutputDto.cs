using System;
using JacksonVeroneze.StockService.Domain;

namespace JacksonVeroneze.StockService.Application.DTO.Output
{
    public class OutputDto
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalValue { get; set; }

        public OutputStateEnum State { get; set; }
    }
}
