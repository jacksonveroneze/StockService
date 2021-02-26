using System;

namespace JacksonVeroneze.StockService.Domain.Filters
{
    public class OutputFilter
    {
        public string Description { get; set; }

        public int? State { get; set; }

        public DateTime? DateInitial { get; set; }

        public DateTime? DateEnd { get; set; }
    }
}
