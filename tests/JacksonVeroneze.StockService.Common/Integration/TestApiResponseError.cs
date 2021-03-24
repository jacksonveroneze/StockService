using System.Collections.Generic;
using System.Net.Http;
using JacksonVeroneze.StockService.Core.Notifications;

namespace JacksonVeroneze.StockService.Common.Integration
{
    public abstract class TestApiResponseError
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int? Status { get; set; }

        public string Detail { get; set; }

        public string Instance { get; set; }

        public IEnumerable<Notification> Errors { get; set; } = new List<Notification>();

        public HttpResponseMessage HttpResponse { get; set; }
    }
}
