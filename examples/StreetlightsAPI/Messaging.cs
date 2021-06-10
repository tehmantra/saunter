using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Saunter.Attributes;

namespace StreetlightsAPI
{
    public class LightMeasuredEvent
    {
        /// <summary>
        /// Id of the streetlight.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Light intensity measured in lumens.
        /// </summary>
        public int Lumens { get; set; }

        /// <summary>
        /// Light intensity measured in lumens.
        /// </summary>
        public DateTime SentAt { get; set; }
    }
    
    public interface IStreetlightMessageBus
    {
        void PublishLightMeasuredEvent(Streetlight streetlight, int lumens);
    }


    [AsyncApi]
    public class StreetlightMessageBus : IStreetlightMessageBus
    {
        private const string PublishLightMeasuredTopic = "publish/light/measured";
        private const string SubscribeLightMeasuredTopic = "subscribe/light/measured";

        private readonly ILogger _logger;

        public StreetlightMessageBus(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("Streetlight");
        }
        
        [Channel(PublishLightMeasuredTopic)]
        [PublishOperation(typeof(LightMeasuredEvent), Summary = "Inform about environmental lighting conditions for a particular streetlight.")]
        public void PublishLightMeasuredEvent(Streetlight streetlight, int lumens)
        {
            var lightMeasuredEvent = new LightMeasuredEvent
            {
                Id = streetlight.Id,
                Lumens = lumens,
                SentAt = DateTime.Now,
            };
            var payload = JsonSerializer.Serialize(lightMeasuredEvent);

            // Simulate publishing a message to the channel.
            // In reality this would call some kind of pub/sub client library and publish.
            // e.g. mqttClient.PublishAsync(message);
            // e.g. amqpClient.BasicPublish(LightMeasuredTopic, routingKey, props, payloadBytes);
            _logger.LogInformation("Publishing message {Payload} to {Topic}", payload, PublishLightMeasuredTopic);
        }

        [Channel(SubscribeLightMeasuredTopic)]
        [SubscribeOperation(typeof(LightMeasuredEvent), Summary = "Subscribe to environmental lighting conditions for a particular streetlight.")]
        public void SubscribeToLightMeasuredEvent(Streetlight streetlight, int lumens)
        {
            var lightMeasuredEvent = new LightMeasuredEvent
            {
                Id = streetlight.Id,
                Lumens = lumens,
                SentAt = DateTime.Now,
            };
            var payload = JsonSerializer.Serialize(lightMeasuredEvent);

            // Simulate subscribing to a channel.
            // In reality this would call some kind of pub/sub client library to subscribe.
            // e.g. amqpClient.BasicConsume(LightMeasuredTopic, ...);
            _logger.LogInformation("Subscribing to {Topic} with payload {Payload} ", SubscribeLightMeasuredTopic, payload);
        }
    }
}