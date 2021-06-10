using System;
using System.Collections.Generic;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Utils
{
    public static class ChannelItemExtensions
    {
        public static void AddRange(this IDictionary<string, ChannelItem> source, IDictionary<string, ChannelItem> channels)
        {
            if (channels == null)
            {
                return;
            }

            foreach (var channel in channels)
            {
                source.AddOrAppend(channel.Key, channel.Value);
            }
        }

        public static void AddOrAppend(this IDictionary<string, ChannelItem> source, string key, ChannelItem channel)
        {
            if (source.ContainsKey(key))
            {
                var existingItem = source[key];
                if (existingItem.Publish == null && channel.Publish != null)
                    source[key].Publish = channel.Publish;
                else if (existingItem.Publish != null && channel.Publish != null)
                    throw new ArgumentException($"An item with the same key and with an existing publish operation has already been added to the channel collection.");

                if (existingItem.Subscribe == null && channel.Subscribe != null)
                    source[key].Subscribe = channel.Subscribe;
                else if (existingItem.Subscribe != null && channel.Subscribe != null)
                    throw new ArgumentException($"An item with the same key and with an existing subscribe operation has already been added to the channel collection.");
            }
            else
                source[key] = channel;
        }
    }
}
