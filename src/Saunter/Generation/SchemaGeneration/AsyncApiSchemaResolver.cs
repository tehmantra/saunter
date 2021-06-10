using System;
using System.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation.SchemaGeneration
{
    public class AsyncApiSchemaResolver : JsonSchemaResolver
    {
        private readonly AsyncApiDocument _document;
        private readonly JsonSchemaGeneratorSettings _settings;

        public AsyncApiSchemaResolver(AsyncApiDocument document, JsonSchemaGeneratorSettings settings) 
            : base(document, settings)
        {
            _document = document;
            _settings = settings;
        }

        public override void AppendSchema(JsonSchema schema, string typeNameHint)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (schema == RootObject)
                throw new ArgumentException("The root schema cannot be appended.");
            
            if (!_document.Components.Schemas.Values.Contains(schema))
            {
                // TODO: how to use our SchemaIdFactory here? Do we need to?
                var schemaId = _settings.TypeNameGenerator.Generate(schema, typeNameHint, _document.Components.Schemas.Keys.Select(k => k.ToString()));

                if (!string.IsNullOrEmpty(schemaId) && !_document.Components.Schemas.ContainsKey(schemaId))
                {
                    _document.Components.Schemas.Add(schemaId, schema);
                    schema.Id = schemaId;
                }
                else
                    _document.Components.Schemas.Add("ref_" + Guid.NewGuid().ToString().Replace("-", "_"), schema);
            }
        }

        public IMessage GetMessageOrReference(Message message)
        {
            var id = message.Name;
            if (id == null)
            {
                return message;
            }

            if (!_document.Components.Messages.ContainsKey(id))
            {
                _document.Components.Messages.Add(id, message);
                message.Payload = new JsonSchema()
                {
                    Reference = message.Payload
                };
            }

            return new MessageReference(id);
        }
    }
}