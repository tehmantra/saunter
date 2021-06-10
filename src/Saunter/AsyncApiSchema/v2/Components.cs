using NJsonSchema;
using System.Collections.Generic;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings;
using Saunter.AsyncApiSchema.v2.Traits;

namespace Saunter.AsyncApiSchema.v2
{
    public class Components
    {
        /// <summary>
        /// An object to hold reusable Schema Objects.
        /// </summary>
        [JsonProperty("schemas", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, JsonSchema> Schemas { get; set; } = new Dictionary<string, JsonSchema>();

        /// <summary>
        /// An object to hold reusable Message Objects.
        /// </summary>
        [JsonProperty("messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, Message> Messages { get; set; } = new Dictionary<string, Message>();

        /// <summary>
        /// An object to hold reusable Security Scheme Objects.
        /// </summary>
        [JsonProperty("securitySchemes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, SecurityScheme>();

        /// <summary>
        /// An object to hold reusable Parameter Objects.
        /// </summary>
        [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, Parameter> Parameters { get; set; } = new Dictionary<string, Parameter>();

        /// <summary>
        /// An object to hold reusable Correlation ID Objects.
        /// </summary>
        [JsonProperty("correlationIds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, CorrelationId> CorrelationIds { get; set; } = new Dictionary<string, CorrelationId>();
        
        /// <summary>
        /// An object to hold reusable Server Binding Objects.
        /// </summary>
        [JsonProperty("serverBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, ServerBindings> ServerBindings { get; set; } = new Dictionary<string, ServerBindings>();

        /// <summary>
        /// An object to hold reusable Channel Binding Objects.
        /// </summary>
        [JsonProperty("channelBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, ChannelBindings> ChannelBindings { get; set; } = new Dictionary<string, ChannelBindings>();

        /// <summary>
        /// An object to hold reusable Operation Binding Objects.
        /// </summary>
        [JsonProperty("operationBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OperationBindings> OperationBindings { get; set; } = new Dictionary<string, OperationBindings>();

        /// <summary>
        /// An object to hold reusable Message Binding Objects.
        /// </summary>
        [JsonProperty("messageBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, MessageBindings> MessageBindings { get; set; } = new Dictionary<string, MessageBindings>();

        
        /// <summary>
        /// An object to hold reusable Operation Trait Objects.
        /// </summary>
        [JsonProperty("operationTraits", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, OperationTrait> OperationTraits { get; set; } = new Dictionary<string, OperationTrait>();

        /// <summary>
        /// An object to hold reusable Message Trait Objects.
        /// </summary>
        [JsonProperty("messageTraits", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, MessageTrait> MessageTraits { get; set; } = new Dictionary<string, MessageTrait>();



        public bool ShouldSerializeMessageTraits()
        {
            return MessageTraits != null && MessageTraits.Count > 0;
        }

        public bool ShouldSerializeOperationTraits()
        {
            return OperationTraits != null && OperationTraits.Count > 0;
        }

        public bool ShouldSerializeMessageBindings()
        {
            return MessageBindings != null && MessageBindings.Count > 0;
        }

        public bool ShouldSerializeOperationBindings()
        {
            return OperationBindings != null && OperationBindings.Count > 0;
        }

        public bool ShouldSerializeChannelBindings()
        {
            return ChannelBindings != null && ChannelBindings.Count > 0;
        }

        public bool ShouldSerializeServerBindings()
        {
            return ServerBindings != null && ServerBindings.Count > 0;
        }

        public bool ShouldSerializeCorrelationIds()
        {
            return CorrelationIds != null && CorrelationIds.Count > 0;
        }

        public bool ShouldSerializeParameters()
        {
            return Parameters != null && Parameters.Count > 0;
        }

        public bool ShouldSerializeSecuritySchemes()
        {
            return SecuritySchemes != null && SecuritySchemes.Count > 0;
        }

        public bool ShouldSerializeMessages()
        {
            return Messages != null && Messages.Count > 0;
        }

        public bool ShouldSerializeSchemas()
        {
            return Schemas != null && Schemas.Count > 0;
        }
    }
}