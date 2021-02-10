using System;

namespace JacksonVeroneze.StockService.Application.DTO.Product
{
    public class ProductResultDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
