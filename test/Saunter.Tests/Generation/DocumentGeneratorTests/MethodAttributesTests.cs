using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation;
using Saunter.Generation.SchemaGeneration;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.DocumentGeneratorTests
{
    public class MethodAttributesTests
    {
        [Fact]
        public void GenerateDocument_GeneratesDocumentWithMultipleMessagesPerChannel()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new JsonSchemaGenerator(options.JsonSchemaGeneratorSettings);
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator, options.JsonSchemaGeneratorSettings);

            // Act
            var document = documentGenerator.GenerateDocument(new []{ typeof(TenantMessagePublisher).GetTypeInfo() });
            
            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.tenants_history");
            channel.Value.Description.ShouldBe("Tenant events.");
            
            var publish = channel.Value.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantMessagePublisher");
            publish.Summary.ShouldBe("Publish domains events about tenants.");

            var messages = publish.Message.ShouldBeOfType<Messages>();
            messages.OneOf.Count.ShouldBe(3);
            
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantCreated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantUpdated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantRemoved");
        }

        [AsyncApi]
        public class TenantMessagePublisher : ITenantMessagePublisher
        {
            [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
            [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
            [Message(typeof(AnyTenantCreated))]
            [Message(typeof(AnyTenantUpdated))]
            [Message(typeof(AnyTenantRemoved))]
            public void PublishTenantEvent<TEvent>(Guid tenantId, TEvent @event)
                where TEvent : IEvent
            {
            }
        }
    }

    public class AnyTenantCreated : IEvent {}
    
    public class AnyTenantUpdated : IEvent {}
    
    public class AnyTenantRemoved : IEvent {}

    public interface IEvent {}
    
    public interface ITenantMessagePublisher
    {
        void PublishTenantEvent<TEvent>(Guid tenantId, TEvent @event)
            where TEvent : IEvent;
    }
}