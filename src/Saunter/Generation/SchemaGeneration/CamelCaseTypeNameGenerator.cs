using System.Collections.Generic;
using NJsonSchema;

namespace Saunter.Generation.SchemaGeneration
{
    public class CamelCaseTypeNameGenerator : DefaultTypeNameGenerator
    {
        public override string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
        {
            return CamelCase(base.Generate(schema, typeNameHint, reservedTypeNames));
        }

        protected override string Generate(JsonSchema schema, string typeNameHint)
        {
            return CamelCase(base.Generate(schema, typeNameHint));
        }

        private string CamelCase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return char.ToLower(name[0]) + name.Substring(1);
        }
    }
}
