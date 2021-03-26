using System;

namespace JacksonVeroneze.StockService.Domain.Models
{
    public class MovementModel
    {
        public Guid ProductId { get; set; }

        public string ProductDescription { get; set; }

        public int Ammount { get; set; }
    }
}
