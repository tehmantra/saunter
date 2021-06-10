using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.Utils
{
    public class DictionaryKeyToStringConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            // Check whether type either IS an IDictionary or implements it
            return TypeIsDictionary(typeToConvert)
                   || TypeIsDictionary(typeToConvert.BaseType); // e.g. class Servers : Dictionary<ServersFieldName, Server>
        }

        private static bool TypeIsDictionary(Type type)
            => type != null
                   && type.IsGenericType
                   && (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) || type.GetInterface("IDictionary") != null);

        public override JsonConverter CreateConverter(
            Type type,
            JsonSerializerOptions options)
        {
            var genericArguments = TypeIsDictionary(type)
                ? type.GetGenericArguments()
                : type.BaseType?.GetGenericArguments();

            if (genericArguments == null || genericArguments.Length != 2)
            {
                throw new NotSupportedException($"Type {type} not supported by this converted.");
            }

            var keyType = genericArguments[0];
            var valueType = genericArguments[1];

            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(DictionaryKeyToStringConverterInner<,>).MakeGenericType(
                    new Type[] { keyType, valueType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null);

            return converter;
        }

        private class DictionaryKeyToStringConverterInner<TKey, TValue> :
            JsonConverter<IDictionary<TKey, TValue>>
        {
            private readonly JsonConverter<TValue> _valueConverter;

            public DictionaryKeyToStringConverterInner(JsonSerializerOptions options)
            {
                // For performance, use the existing converter if available.
                _valueConverter = (JsonConverter<TValue>)options
                    .GetConverter(typeof(TValue));
            }

            public override IDictionary<TKey, TValue> Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                // Doesn't support deserialization
                throw new NotSupportedException();
            }

            public override void Write(
                Utf8JsonWriter writer,
                IDictionary<TKey, TValue> dictionary,
                JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
                {
                    writer.WritePropertyName(kvp.Key.ToString());

                    if (_valueConverter != null)
                        _valueConverter.Write(writer, kvp.Value, options);
                    else
                        JsonSerializer.Serialize(writer, kvp.Value, options);
                }

                writer.WriteEndObject();
            }
        }
    }
}