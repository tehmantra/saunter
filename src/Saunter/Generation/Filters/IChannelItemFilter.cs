using System;
using System.Reflection;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Generation.Filters
{
    public interface IChannelItemFilter
    {
        void Apply(ChannelItem channelItem, ChannelItemFilterContext context);
    }

    public class ChannelItemFilterContext
    {
        public ChannelItemFilterContext(MemberInfo member, JsonSchemaResolver schemaResolver, ChannelAttribute channel)
        {
            Member = member;
            SchemaResolver = schemaResolver;
            Channel = channel;
        }
        
        public MemberInfo Member { get; }
        public JsonSchemaResolver SchemaResolver { get; }

        public ChannelAttribute Channel { get; }
    }
}