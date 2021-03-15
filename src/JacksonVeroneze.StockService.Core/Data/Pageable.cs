using System.Collections.Generic;

namespace JacksonVeroneze.StockService.Core.Data
{
    public class Pageable<T> where T : class
    {
        public int? Total { get; set; }

        public int? Pages { get; set; }

        public int? CurrentPage { get; set; }

        public IList<T> Data { get; set; }
    }
}
