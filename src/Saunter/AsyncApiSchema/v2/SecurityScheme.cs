using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class SecurityScheme
    {
        public SecurityScheme(SecuritySchemeType type)
        {
            Type = type;
        }

        /// <summary>
        /// The type of the security scheme.
        /// </summary>
        [JsonProperty("type")]
        public SecuritySchemeType Type { get; }

        /// <summary>
        /// A short description for security scheme.
        /// CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the header, query or cookie parameter to be used.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The location of the API key.
        /// Valid values are "user" and "password" for apiKey and "query", "header" or "cookie" for httpApiKey.
        /// </summary>
        [JsonProperty("in")]
        public string In { get; set; }

        /// <summary>
        /// The name of the HTTP Authorization scheme to be used in the Authorization header as defined in RFC7235.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        /// <summary>
        /// A hint to the client to identify how the bearer token is formatted. Bearer tokens are usually generated
        /// by an authorization server, so this information is primarily for documentation purposes.
        /// </summary>
        [JsonProperty("bearerFormat")]
        public string BearerFormat { get; set; }

        /// <summary>
        /// An object containing configuration information for the flow types supported.
        /// </summary>
        [JsonProperty("flows")]
        public OAuthFlows Flows { get; set; }

        /// <summary>
        /// OpenId Connect URL to discover OAuth2 configuration values. This MUST be in the form of a URL.
        /// </summary>
        [JsonProperty("openIdConnectUrl")]
        public string OpenIdConnectUrl { get; set; }
    }
}