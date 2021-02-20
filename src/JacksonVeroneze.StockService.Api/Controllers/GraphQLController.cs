using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQL.Validation.Complexity;
using JacksonVeroneze.StockService.Api.Graphql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class GraphQLController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecutor;
        private readonly IDocumentWriter _documentWriter;
        private readonly ISchema _schema;
        private readonly IServiceProvider _serviceProvider;

        public GraphQLController(IDocumentExecuter documentExecuter, ISchema schema,
            IServiceProvider serviceProvider, IDocumentWriter documentWriter)
        {
            _documentExecutor = documentExecuter;
            _schema = schema;
            _serviceProvider = serviceProvider;
            _documentWriter = documentWriter;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] GraphQLQuery request)
        {
            ExecutionResult result = await _documentExecutor.ExecuteAsync(x =>
            {
                x.Schema = _schema;
                x.Query = request.Query;
                x.Inputs = request.Variables?.ToObject<Inputs>();
                x.ComplexityConfiguration = new ComplexityConfiguration {MaxDepth = 15};
            }).ConfigureAwait(false);

            return Ok(await _documentWriter.WriteToStringAsync(result));
        }
    }
}
