using System;
using JacksonVeroneze.StockService.Domain.Enums;

namespace JacksonVeroneze.StockService.Application.DTO.Devolution
{
    public class DevolutionDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public DevolutionState State { get; set; }
    }
}
