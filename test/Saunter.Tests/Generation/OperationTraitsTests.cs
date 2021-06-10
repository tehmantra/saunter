using Microsoft.Extensions.DependencyInjection;
using Saunter.AsyncApiSchema.v2;
using Saunter.AsyncApiSchema.v2.Traits;
using Saunter.Generation;
using Saunter.Generation.Filters;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation
{
    public class OperationTraitsTests
    {
        [Fact]
        public void Example_OperationTraits()
        {
            // TODO: this is not really a test, just an example of how you might use OperationTraits...

            var services = new ServiceCollection() as IServiceCollection;
            services.AddAsyncApiSchemaGeneration(o =>
            {
                o.AsyncApi = new AsyncApiDocument
                {
                    Components =
                    {
                        OperationTraits =
                        {
                            ["exampleTrait"] = new OperationTrait { Description = "This is an example trait" }
                        }
                    }
                };

                o.OperationFilters.Add(new TestOperationTraitsFilter());
            });

            using (var serviceprovider = services.BuildServiceProvider())
            {
                var documentProvider = serviceprovider.GetRequiredService<IAsyncApiDocumentProvider>();
                var document = documentProvider.GetDocument();

                document.Components.OperationTraits.ShouldContainKey("exampleTrait");
            }
        }
        
        
        private class TestOperationTraitsFilter : IOperationFilter
        {
            public void Apply(Operation publishOperation, OperationFilterContext context)
            {
                publishOperation.Traits.Add(new OperationTraitReference("exampleTrait"));
            }
        }
    }
}