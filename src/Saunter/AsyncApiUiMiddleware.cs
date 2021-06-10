#if !NETSTANDARD2_0

using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2;
using Saunter.Utils;

namespace Saunter
{
    public class AsyncApiUiMiddleware
    {

        private const string GenerateApiPath = "html/generate";
        private const string PlaygroundAssetsPath = "/html/template/";
        private const string PlaygroundHtmlCacheKey = "index.html";

        private readonly IOptions<AsyncApiOptions> _options;
        private readonly HttpClient _client;
        private readonly IMemoryCache _memoryCache;
        
        public AsyncApiUiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options)
        {
            _options = options;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_options.Value.Middleware.PlaygroundBaseAddress)
            };
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task Invoke(HttpContext context, IAsyncApiDocumentProvider asyncApiDocumentProvider, IAsyncApiDocumentSerializer asyncApiDocumentSerializer)
        {
            if (IsRequestingAsyncApiUi(context.Request))
            {
                var asyncApiSchema = asyncApiDocumentProvider.GetDocument();
                await RespondWithAsyncApiHtml(context.Response, asyncApiSchema, asyncApiDocumentSerializer);
                return;
            }

            await RespondWithProxiedResponse(context.Request, context.Response);
        }

        private async Task RespondWithAsyncApiHtml(HttpResponse response, AsyncApiDocument asyncApiSchema, IAsyncApiDocumentSerializer asyncApiDocumentSerializer)
        {
            var asyncApiHtml = await _memoryCache.GetOrCreateAsync(PlaygroundHtmlCacheKey, async _ =>
            {
                var asyncApiSchemaJson = asyncApiDocumentSerializer.Serialize(asyncApiSchema);
                
                var asyncApiHtmlResponse = await _client.PostAsync(GenerateApiPath, new StringContent(asyncApiSchemaJson));
                
                return await asyncApiHtmlResponse.Content.ReadAsStringAsync();
            });

            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = MediaTypeNames.Text.Html;
            
            await response.WriteAsync(asyncApiHtml);
        }

        private async Task RespondWithProxiedResponse(HttpRequest request, HttpResponse response)
        {
            var resource = await _memoryCache.GetOrCreateAsync(request.Path, async _ =>
            {
                // Strip off the path this middleware is hosted at
                var playgroundPath = request.Path.Value.Replace(_options.Value.Middleware.UiBaseRoute, PlaygroundAssetsPath);

                var proxiedResponse = await _client.GetAsync(playgroundPath);

                return await proxiedResponse.Content.ReadAsStringAsync();
            });
            
            response.StatusCode = (int)HttpStatusCode.OK;
            
            await response.WriteAsync(resource);
        }

        
        private bool IsRequestingAsyncApiUi(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path, _options.Value.Middleware.UiRoute, StringComparison.OrdinalIgnoreCase);
        }
    }
}
#endif