using Saunter.AsyncApiSchema.v2;

namespace Saunter.Utils
{
    public interface IAsyncApiDocumentSerializer
    {
        string ContentType { get; }

        string Serialize(AsyncApiDocument document);
    }
}