using Newtonsoft.Json.Linq;

namespace JacksonVeroneze.StockService.Api.Graphql
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }

        public string NamedQuery { get; set; }

        public string Query { get; set; }

        public JObject Variables { get; set; }
    }
}
