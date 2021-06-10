using NJsonSchema;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// Can be either a <see cref="Parameter"/> or a <see cref="ParameterReference"/> reference to a parameter.
    /// </summary>
    public interface IParameter { }

    /// <summary>
    /// A reference to a Parameter within the AsyncAPI components.
    /// </summary>
    public class ParameterReference : Reference, IParameter
    {
        public ParameterReference(string id) : base(id, "#/components/parameters/{0}") { }
    }
    
    public class Parameter : IParameter
    {
        /// <summary>
        /// A verbose explanation of the parameter.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        /// <summary>
        /// Definition of the parameter.
        /// </summary>
        [JsonProperty("schema")]
        public JsonSchema Schema { get; set; }

        /// <summary>
        /// A runtime expression that specifies the location of the parameter value.
        /// Even when a definition for the target field exists, it MUST NOT be used to validate
        /// this parameter but, instead, the schema property MUST be used.
        /// </summary>
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }
    }
}