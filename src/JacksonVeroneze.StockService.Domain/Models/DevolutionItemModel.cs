using System;

namespace JacksonVeroneze.StockService.Domain.Models
{
    public class DevolutionItemModel
    {
        public Guid Id { get; set; }

        public int Amount { get; set; }

        public Guid DevolutionId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductDescription { get; set; }
    }
}
