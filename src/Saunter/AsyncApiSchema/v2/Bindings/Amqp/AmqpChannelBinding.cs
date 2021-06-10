using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Saunter.AsyncApiSchema.v2.Bindings.Amqp
{
    /// <remarks>
    /// https://github.com/asyncapi/bindings/blob/master/amqp/README.md#channel
    /// </remarks>
    public class AmqpChannelBinding
    {
        [JsonProperty("is")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AmqpChannelBindingIs Is { get; set; }

        [JsonProperty("exchange")]
        public AmqpChannelBindingExchange Exchange { get; set; }

        [JsonProperty("queue")]
        public AmqpChannelBindingQueue Queue { get; set; }

        [JsonProperty("bindingVersion")]
        public string BindingVersion { get; set; }
    }

    public class AmqpChannelBindingExchange
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("durable")]
        public bool? Durable { get; set; }

        [JsonProperty("autoDelete")]
        public bool? AutoDelete { get; set; }

        [JsonProperty("vhost")]
        public string VirtualHost { get; set; }
    }

    public class AmqpChannelBindingQueue
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("durable")]
        public bool? Durable { get; set; }

        [JsonProperty("exclusive")]
        public bool? Exclusive { get; set; }

        [JsonProperty("autoDelete")]
        public bool? AutoDelete { get; set; }

        [JsonProperty("vhost")]
        public string VirtualHost { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum AmqpChannelBindingIs
    {
        [EnumMember(Value = "routingKey")]
        RoutingKey,

        [EnumMember(Value = "queue")]
        Queue,
    }

    public static class AmqpChannelBindingExchangeType
    {
        public const string Topic = "topic";

        public const string Direct = "direct";

        public const string Fanout = "fanout";

        public const string Default = "default";

        public const string Headers = "headers";
    }
}