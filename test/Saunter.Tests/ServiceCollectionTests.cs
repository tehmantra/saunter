using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Shouldly;
using Xunit;

namespace Saunter.Tests
{
    /// <remarks>
    /// These are not real tests! Tests will be added once the API is semi-stable...
    /// </remarks>
    public class ServiceCollectionTests
    {

        [Fact]
        public void TestAddAsyncApiSchemaGeneration()
        {
            var services = new ServiceCollection() as IServiceCollection;
            services.AddAsyncApiSchemaGeneration(options =>
                {
                    options.AsyncApi = new AsyncApiSchema.v2.AsyncApiDocument
                    {
                        Id = "urn:com:example:example-events",
                        Info = new Info("Example API", "2019.01.12345")
                        {
                            Description = "An example API with events",
                            Contact = new Contact
                            {
                                Email = "michael@mwild.me",
                                Name = "Michael Wildman",
                                Url = "https://mwild.me/",
                            },
                            License = new License("MIT"),
                            TermsOfService = "https://mwild.me/tos",
                        },
                        Tags = { "example", "event" },
                        Servers =
                        {
                            { 
                                "development", 
                                new Server("rabbitmq.dev.mwild.me", "amqp")
                                {
                                    Security = new List<Dictionary<string, List<string>>> { new Dictionary<string, List<string>> { { "user-password", new List<string>() } }}
                                }
                            }
                        },
                        Components =
                        {
                            SecuritySchemes = new Dictionary<string, SecurityScheme>
                            {
                                { "user-password", new SecurityScheme(SecuritySchemeType.Http) }
                            }
                        }
                    };
                });

            var sp = services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IAsyncApiDocumentProvider>();

            var document = provider.GetDocument();

            document.ShouldNotBeNull();
        }
    }
}
