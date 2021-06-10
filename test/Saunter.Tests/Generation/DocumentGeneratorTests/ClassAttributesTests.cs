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
    public class ClassAttributesTests
    {
        [Fact]
        public void GetDocument_GeneratesDocumentWithMultipleMessagesPerChannel()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new JsonSchemaGenerator(options.JsonSchemaGeneratorSettings);
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator, options.JsonSchemaGeneratorSettings);
            
            // Act
            var document = documentGenerator.GenerateDocument(new[] { typeof(TenantMessageConsumer).GetTypeInfo() });

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.tenants_history");
            channel.Value.Description.ShouldBe("Tenant events.");

            var subscribe = channel.Value.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("TenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about tenants.");

            var messages = subscribe.Message.ShouldBeOfType<Messages>();
            messages.OneOf.Count.ShouldBe(3);

            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
        }


        [Fact]
        public void GenerateDocument_GeneratesDocumentWithMultipleMessagesPerChannelInTheSameMethod()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new JsonSchemaGenerator(options.JsonSchemaGeneratorSettings);
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator, options.JsonSchemaGeneratorSettings);

            // Act
            var document = documentGenerator.GenerateDocument(new []{ typeof(TenantGenericMessagePublisher).GetTypeInfo() });

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


        [Fact]
        public void GenerateDocument_GeneratesDocumentWithSingleMessage()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new JsonSchemaGenerator(options.JsonSchemaGeneratorSettings);
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator, options.JsonSchemaGeneratorSettings);

            // Act
            var document = documentGenerator.GenerateDocument(new []{ typeof(TenantSingleMessagePublisher).GetTypeInfo() });

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.tenants_history");
            channel.Value.Description.ShouldBe("Tenant events.");
            
            var publish = channel.Value.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantSingleMessagePublisher");
            publish.Summary.ShouldBe("Publish single domain event about tenants.");

            var message = publish.Message.ShouldBeOfType<MessageReference>();
            message.Id.ShouldBe("anyTenantCreated");
        }


        [Fact]
        public void GetDocument_WhenMultipleClassesUseSameChannelKey_GeneratesDocumentWithMultipleMessagesPerChannel()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new JsonSchemaGenerator(options.JsonSchemaGeneratorSettings);
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator, options.JsonSchemaGeneratorSettings);

            // Act
            var document = documentGenerator.GenerateDocument(new[]
            {
                typeof(TenantMessageConsumer).GetTypeInfo(),
                typeof(TenantMessagePublisher).GetTypeInfo()
            });

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.tenants_history");
            channel.Value.Description.ShouldBe("Tenant events.");

            var subscribe = channel.Value.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("TenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about tenants.");

            var publish = channel.Value.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantMessagePublisher");
            publish.Summary.ShouldBe("Publish domains events about tenants.");


            var subscribeMessages = subscribe.Message.ShouldBeOfType<Messages>();
            subscribeMessages.OneOf.Count.ShouldBe(3);

            subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
            subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
            subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");

            var publishMessages = subscribe.Message.ShouldBeOfType<Messages>();
            publishMessages.OneOf.Count.ShouldBe(3);

            publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
            publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
            publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
        }


        [Fact]
        public void GenerateDocument_GeneratesDocumentWithChannelParameters()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new JsonSchemaGenerator(options.JsonSchemaGeneratorSettings);
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator, options.JsonSchemaGeneratorSettings);

            // Act
            var document = documentGenerator.GenerateDocument(new []{ typeof(OneTenantMessageConsumer).GetTypeInfo() });
            
            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.{tenant_id}.{tenant_status}");
            channel.Value.Description.ShouldBe("A tenant events.");
            channel.Value.Parameters.Count.ShouldBe(2);
            channel.Value.Parameters.Values.OfType<Parameter>().ShouldContain(p => p.Name == "tenant_id" && p.Schema != null && p.Description == "The tenant identifier.");
            channel.Value.Parameters.Values.OfType<Parameter>().ShouldContain(p => p.Name == "tenant_status" && p.Schema != null && p.Description == "The tenant status.");
            
            var subscribe = channel.Value.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("OneTenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about a tenant.");

            var messages = subscribe.Message.ShouldBeOfType<Messages>();
            messages.OneOf.Count.ShouldBe(3);
            
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
        }
        

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [SubscribeOperation(OperationId = "TenantMessageConsumer", Summary = "Subscribe to domains events about tenants.")]
        public class TenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            public void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt) { }

            [Message(typeof(TenantUpdated))]
            public void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) { }

            [Message(typeof(TenantRemoved))]
            public void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) { }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        public class TenantMessagePublisher
        {
            [Message(typeof(TenantCreated))]
            public void PublishTenantCreatedEvent(Guid tenantId, TenantCreated evnt) { }

            [Message(typeof(TenantUpdated))]
            public void PublishTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) { }

            [Message(typeof(TenantRemoved))]
            public void PublishTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) { }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        public class TenantGenericMessagePublisher
        {
            [Message(typeof(AnyTenantCreated))]
            [Message(typeof(AnyTenantUpdated))]
            [Message(typeof(AnyTenantRemoved))]
            public void PublishTenantEvent<TEvent>(Guid tenantId, TEvent @event)
                where TEvent : IEvent
            {
            }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantSingleMessagePublisher", Summary = "Publish single domain event about tenants.")]
        public class TenantSingleMessagePublisher
        {
            [Message(typeof(AnyTenantCreated))]
            public void PublishTenantCreated(Guid tenantId, AnyTenantCreated @event)
            {
            }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.{tenant_id}.{tenant_status}", Description = "A tenant events.")]
        [ChannelParameter("tenant_id", typeof(long), Description = "The tenant identifier.")]
        [ChannelParameter("tenant_status", typeof(string), Description = "The tenant status.")]
        [SubscribeOperation(OperationId = "OneTenantMessageConsumer", Summary = "Subscribe to domains events about a tenant.")]
        public class OneTenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            public void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt) {}

            [Message(typeof(TenantUpdated))]
            public void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) {}

            [Message(typeof(TenantRemoved))]
            public void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) {}
        }
    }

    public class TenantCreated { }

    public class TenantUpdated { }

    public class TenantRemoved { }
}