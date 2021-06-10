using System.Collections.Generic;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings;
using Saunter.AsyncApiSchema.v2.Traits;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// Describes a publish or a subscribe operation.
    /// This provides a place to document how and why messages are sent and received. 
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Unique string used to identify the operation.
        /// The id MUST be unique among all operations described in the API.
        /// The operationId value is case-sensitive.
        /// Tools and libraries MAY use the operationId to uniquely identify an operation,
        /// therefore, it is RECOMMENDED to follow common programming naming conventions.
        /// </summary>
        [JsonProperty("operationId", NullValueHandling = NullValueHandling.Ignore)]
        public string OperationId { get; set; }

        /// <summary>
        /// A short summary of what the operation is about.
        /// </summary>
        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the operation.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of operations.
        /// </summary>
        [JsonProperty("tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();

        public bool ShouldSerializeTags()
        {
            return Tags != null && Tags.Count > 0;
        }

        /// <summary>
        /// Additional external documentation for this operation.
        /// </summary>
        [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// A free-form map where the keys describe the name of the protocol and the values describe
        /// protocol-specific definitions for the operation.
        /// </summary>
        [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
        public OperationBindings Bindings { get; set; }
        
        /// <summary>
        /// A definition of the message that will be published or received on this channel.
        /// oneOf is allowed here to specify multiple messages, however, a message MUST be
        /// valid only against one of the referenced message objects.
        /// </summary>
        [JsonProperty("message")]
        public IMessage Message { get; set; }

        /// <summary>
        /// A list of traits to apply to the operation object. Traits MUST be merged into the operation
        /// object using the JSON Merge Patch algorithm in the same order they are defined here.
        /// </summary>
        [JsonProperty("traits", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<IOperationTrait> Traits { get; set; } = new List<IOperationTrait>();


        public bool ShouldSerializeTraits()
        {
            return Traits != null && Traits.Count > 0;
        }
    }
}