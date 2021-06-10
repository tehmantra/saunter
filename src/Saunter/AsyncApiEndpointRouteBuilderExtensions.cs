#if !NETSTANDARD2_0
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Saunter
{
    public static class AsyncApiEndpointRouteBuilderExtensions
    {
        private const string AsyncApiDefaultDocumentTitle = "Async API Documentation";

        /// <summary>
        /// Maps the AsyncAPI endpoint.
        /// </summary>
        /// <param name="endpoints">The endpoints.</param>
        /// <param name="title">The document tile.</param>
        /// <returns>The endpoints with AsyncAPI endpoint added.</returns>
        public static IEndpointConventionBuilder MapAsyncApiDocuments(
            this IEndpointRouteBuilder endpoints,
            string title = AsyncApiDefaultDocumentTitle)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<AsyncApiMiddleware>()
                .Build();

            var options = endpoints.ServiceProvider.GetService<IOptions<AsyncApiOptions>>();
            var route = options.Value.Middleware.Route;

            return endpoints.MapGet(route, pipeline).WithDisplayName(title);
        }


        public static IEndpointConventionBuilder MapAsyncApiUi(
            this IEndpointRouteBuilder endpoints,
            string title = AsyncApiDefaultDocumentTitle)
        {
            // Add the middleware 
            var pipeline = endpoints.CreateApplicationBuilder()
                // I don't really understand why...
                // https://github.com/dotnet/aspnetcore/issues/24252#issuecomment-663620294
                .Use((context, next) =>
                {
                    context.SetEndpoint(null);
                    return next();
                })
                .UseMiddleware<AsyncApiUiMiddleware>()
                .Build();

            var options = endpoints.ServiceProvider.GetService<IOptions<AsyncApiOptions>>();
            var route = options.Value.Middleware.UiBaseRoute + "{*wildcard}";

            return endpoints.MapGet(route, pipeline).WithDisplayName(title);
        }
    }
}
#endif