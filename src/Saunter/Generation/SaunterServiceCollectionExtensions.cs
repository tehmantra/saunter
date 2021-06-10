using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NJsonSchema.Generation;
using Saunter.Utils;

namespace Saunter.Generation
{
    public static class SaunterServiceCollectionExtensions
    {
        public static IServiceCollection AddAsyncApiSchemaGeneration(this IServiceCollection services, Action<AsyncApiOptions> setupAction)
        {
            services.AddOptions();

            services.TryAddTransient<IAsyncApiDocumentProvider, AsyncApiDocumentProvider>();
            services.TryAddTransient<IDocumentGenerator, DocumentGenerator>();
            services.TryAddTransient<JsonSchemaGenerator>();
            services.TryAddTransient(c => c.GetRequiredService<IOptions<AsyncApiOptions>>().Value.JsonSchemaGeneratorSettings);
            services.TryAddTransient<IAsyncApiDocumentSerializer, NewtonsoftAsyncApiDocumentSerializer>();

            if (setupAction != null) services.Configure(setupAction);

            return services;
        }
    }
}