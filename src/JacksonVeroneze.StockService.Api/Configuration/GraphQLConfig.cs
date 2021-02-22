using GraphQL;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using JacksonVeroneze.StockService.Api.Graphql.Schema;
using JacksonVeroneze.StockService.Api.Graphql.Schema.CommonSchema;
using JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.StockService.Api.Configuration
{
    public static class GraphQLConfig
    {
        public static IServiceCollection AddGraphQLConfiguration(this IServiceCollection services)
        {
            services.AddTransient<StockSchema>();
            services.AddTransient<QueryType>();
            services.AddTransient<MutationType>();

            services.AddTransient<IDocumentExecuter, DocumentExecuter>();
            services.AddTransient<IDocumentWriter, DocumentWriter>();
            services.AddTransient<ISchema, StockSchema>();

            services.AddTransient<PurchaseInputType>();
            services.AddTransient<PurchaseFilterInputType>();
            services.AddTransient<CommonPaginateInputType>();
            services.AddTransient<PurchaseMutationType>();
            services.AddTransient<PurchaseQueryType>();
            services.AddTransient<PurchaseType>();

            services.AddGraphQL((options, provider) =>
                {
                    options.EnableMetrics = false;
                    var logger = provider.GetRequiredService<ILogger<Startup>>();
                    options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occurred", ctx.OriginalException.Message);
                })
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddDataLoader()
                .AddGraphTypes(typeof(StockSchema));

            return services;
        }

        public static IApplicationBuilder UseGraphQLSetup(this IApplicationBuilder app)
        {
            app.UseGraphQL<StockSchema>();

            return app;
        }
    }
}
