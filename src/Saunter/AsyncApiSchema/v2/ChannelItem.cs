using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// Describes the operations available on a single channel.
    /// </summary>
    public class ChannelItem
    {
        /// <summary>
        /// An optional description of this channel item.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// A definition of the SUBSCRIBE operation.
        /// </summary>
        [JsonProperty("subscribe", NullValueHandling = NullValueHandling.Ignore)]
        public Operation Subscribe { get; set; }

        /// <summary>
        /// A definition of the PUBLISH operation.
        /// </summary>
        [JsonProperty("publish", NullValueHandling = NullValueHandling.Ignore)]
        public Operation Publish { get; set; }

        /// <summary>
        /// A map of the parameters included in the channel name.
        /// It SHOULD be present only when using channels with expressions
        /// (as defined by RFC 6570 section 2.2).
        /// </summary>
        [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, IParameter> Parameters { get; set; } = new Dictionary<string, IParameter>();

        /// <summary>
        /// A free-form map where the keys describe the name of the protocol
        /// and the values describe protocol-specific definitions for the channel.
        /// </summary>
        [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
        public ChannelBindings Bindings { get; set; }


        public bool ShouldSerializeParameters()
        {
            return Parameters != null && Parameters.Count > 0;
        }
    }
}