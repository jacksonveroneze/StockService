using System;

namespace JacksonVeroneze.StockService.Application.DTO.OutputItem
{
    public class OutputItemDto
    {
        public Guid Id { get; set; }

        public int Amount { get; set; }

        public Guid OutputId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductDescription { get; set; }
    }
}
