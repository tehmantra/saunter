using System.Collections.Generic;
using Newtonsoft.Json;
using NJsonSchema.Converters;

namespace Saunter.AsyncApiSchema.v2
{
    [JsonConverter(typeof(JsonReferenceConverter))]
    public class AsyncApiDocument
    {
        /// <summary>
        /// Specifies the AsyncAPI Specification version being used.
        /// </summary>
        [JsonProperty("asyncapi", NullValueHandling = NullValueHandling.Ignore)]
        public string AsyncApi { get; } = "2.0.0";
        
        /// <summary>
        /// Identifier of the application the AsyncAPI document is defining.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public Info Info { get; set; }

        /// <summary>
        /// Provides connection details of servers.
        /// </summary>
        [JsonProperty("servers")]
        public Dictionary<string, Server> Servers { get; } = new Dictionary<string, Server>();

        /// <summary>
        /// A string representing the default content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json).
        /// </summary>
        [JsonProperty("defaultContentType", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultContentType { get; set; } = "application/json";

        /// <summary>
        /// The available channels and messages for the API.
        /// </summary>
        [JsonProperty("channels")]
        public IDictionary<string, ChannelItem> Channels { get; set; } = new Dictionary<string, ChannelItem>();

        /// <summary>
        /// An element to hold various schemas for the specification.
        /// </summary>
        [JsonProperty("components")]
        public Components Components { get; set; } = new Components();

        /// <summary>
        /// A list of tags used by the specification with additional metadata.
        /// Each tag name in the list MUST be unique.
        /// </summary>
        [JsonProperty("tags")]
        public ISet<Tag> Tags { get; } = new HashSet<Tag>();

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
        public ExternalDocumentation ExternalDocs { get; set; }



        public bool ShouldSerializeTags()
        {
            return Tags != null && Tags.Count > 0;
        }

        public bool ShouldSerializeServers()
        {
            return Servers != null && Servers.Count > 0;
        }
    }
}