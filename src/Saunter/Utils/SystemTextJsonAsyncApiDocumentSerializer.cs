using System;
using Saunter.AsyncApiSchema.v2;
using System.Text.Json;

namespace Saunter.Utils
{
    [Obsolete(@" note for review
according to https://github.com/RicoSuter/NSwag/issues/2243
    System.Text.Json can only be used to generate a schema but not to serialize it
")]
    public class SystemTextJsonAsyncApiDocumentSerializer : IAsyncApiDocumentSerializer
    {
        public string Serialize(AsyncApiDocument document)
        {
            return JsonSerializer.Serialize(
                document,
                new JsonSerializerOptions
                {
                    WriteIndented = false,
                    IgnoreNullValues = true,
                    Converters =
                    {
                        new DictionaryKeyToStringConverter(),
                        new InterfaceImplementationConverter(),
                    },
                }
            );
        }

        public string ContentType => "application/json";
    }
}