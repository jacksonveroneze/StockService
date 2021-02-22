using GraphQL;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using JacksonVeroneze.StockService.Api.Graphql.Schema;
using JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class GraphQLConfig
    {
        public static IServiceCollection AddGraphQLConfiguration(this IServiceCollection services)
        {
            services.AddTransient<QueryType>();
            services.AddTransient<MutationType>();

            services.AddTransient<IDocumentExecuter, DocumentExecuter>();
            services.AddTransient<IDocumentWriter, DocumentWriter>();
            services.AddTransient<ISchema, Graphql.Schema.Schema>();

            services.AddTransient<PurchaseInputType>();
            services.AddTransient<PurchaseFilterInputType>();
            services.AddTransient<PurchasePaginateInputType>();
            services.AddTransient<PurchaseMutationType>();
            services.AddTransient<PurchaseQueryType>();
            services.AddTransient<PurchaseType>();

            return services;
        }
    }
}
