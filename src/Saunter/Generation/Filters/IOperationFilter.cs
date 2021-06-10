using System;
using System.Reflection;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Generation.Filters
{
    public interface IOperationFilter
    {
        void Apply(Operation operation, OperationFilterContext context);
    }

    public class OperationFilterContext
    {
        public OperationFilterContext(MethodInfo method, JsonSchemaResolver schemaResolver, OperationAttribute operation)
        {
            Method = method;
            SchemaResolver = schemaResolver;
            Operation = operation;
        }
        
        public MethodInfo Method { get; }
        public JsonSchemaResolver SchemaResolver { get; }
        
        public OperationAttribute Operation { get; }
    }
}