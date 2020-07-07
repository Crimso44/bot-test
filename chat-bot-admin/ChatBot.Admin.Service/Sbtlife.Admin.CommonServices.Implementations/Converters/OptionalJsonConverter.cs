using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ChatBot.Admin.Common.Exceptions;
using ChatBot.Admin.Common.Optional;

namespace ChatBot.Admin.CommonServices.Converters
{
    public class OptionalJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetTypeInfo().IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Undefined)
                return existingValue;

            var optionalValueType = objectType.GenericTypeArguments.First();
            var optionalValueTypeInfo = optionalValueType.GetTypeInfo();

            if (reader.TokenType == JsonToken.Null)
            {
                if (optionalValueTypeInfo.IsClass || IsNullable(optionalValueTypeInfo))
                    return Activator.CreateInstance(objectType, new object[] { null });

                throw new OptionalException("Expected non-nullable value on deserialization");
            }

            if (IsNullable(optionalValueTypeInfo))
                optionalValueType = Nullable.GetUnderlyingType(optionalValueType);

            var optionalValue = optionalValueType == typeof(Guid)
                ? new Guid((string)Convert.ChangeType(reader.Value, typeof(string)))
                : Convert.ChangeType(reader.Value, optionalValueType);

            return Activator.CreateInstance(objectType, optionalValue);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var token = JToken.FromObject(value);

            if (token.Type != JTokenType.Object)
                throw new ArgumentException(nameof(value));

            var o = (JObject)token;
            var valueProperty = o.Properties().Single(p => p.Name == "Value");
            valueProperty.Value.WriteTo(writer);
        }

        private bool IsNullable(TypeInfo typeInfo)
        {
            return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
