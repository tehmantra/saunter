using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation.Filters;
using Saunter.Generation.SchemaGeneration;

namespace Saunter
{
    public class AsyncApiOptions
    {
        /// <summary>
        /// The base asyncapi schema. This will be augmented with other information auto-discovered
        /// from attributes.
        /// </summary>
        public AsyncApiDocument AsyncApi { get; set; } = new AsyncApiDocument();

        /// <summary>
        ///     A list of marker types from assemblies to scan for Saunter attributes.
        /// </summary>
        public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();
        
        /// <summary>
        /// A list of filters that will be applied to the generated AsyncAPI document.
        /// </summary>
        public IList<IDocumentFilter> DocumentFilters { get; } = new List<IDocumentFilter>();

        /// <summary>
        /// A list of filters that will be applied to any generated channels.
        /// </summary>
        public IList<IChannelItemFilter> ChannelItemFilters { get; } = new List<IChannelItemFilter>();

        /// <summary>
        /// A list of filters that will be applied to any generated Publish operations.
        /// </summary>
        public IList<IOperationFilter> OperationFilters { get; } = new List<IOperationFilter>();

        /// <summary>
        /// Options related to the Saunter middleware
        /// </summary>
        public AsyncApiMiddlewareOptions Middleware { get; set; } = new AsyncApiMiddlewareOptions();

        public JsonSchemaGeneratorSettings JsonSchemaGeneratorSettings { get; set; } = new JsonSchemaGeneratorSettings()
        {
            TypeNameGenerator = new CamelCaseTypeNameGenerator(),
            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            },
        };
    }

    public class AsyncApiMiddlewareOptions
    {
        /// <summary>
        /// The route which the AsyncAPI document will be hosted
        /// </summary>
        public string Route { get; set; } = "/asyncapi/asyncapi.json";

        /// <summary>
        /// The route which the AsyncAPI UI will be hosted
        /// </summary>
        public string UiRoute { get; set; } = "/asyncapi/ui/index.html";

        /// <summary>
        /// The base URL for the AsyncAPI UI
        /// </summary>
        public string UiBaseRoute { get; set; } = "/asyncapi/ui/";

        /// <summary>
        /// The address of an AsyncAPI playground which will be used to generate the HTML from the AsyncAPI document.
        /// </summary>
        public string PlaygroundBaseAddress { get; set; } = "https://playground.asyncapi.io/";
    }
}