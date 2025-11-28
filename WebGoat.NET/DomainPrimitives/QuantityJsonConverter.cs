using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebGoatCore.DomainPrimitives
{
    public class QuantityJsonConverter : JsonConverter<Quantity>
    {
        public override void WriteJson(JsonWriter writer, Quantity? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
                return;
            }

            // Emit the underlying short numeric value. This keeps session JSON compact and
            // matches the DB mapping which stores Quantity.Value as a numeric column.
            writer.WriteValue(value.Value);
        }

        public override Quantity? ReadJson(JsonReader reader, Type objectType, Quantity? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Handle nulls
            if (reader.TokenType == JsonToken.Null)
                return null;

            // Load token to support either a primitive integer token or an object like { "Value": 3 }
            var token = JToken.Load(reader);

            short numericValue;
            if (token.Type == JTokenType.Object)
            {
                var v = token["Value"];
                if (v == null)
                    throw new JsonSerializationException("Missing 'Value' while deserializing Quantity");

                numericValue = v.Value<short>();
            }
            else if (token.Type == JTokenType.Integer)
            {
                numericValue = token.Value<short>();
            }
            else
            {
                throw new JsonSerializationException($"Unexpected token type {token.Type} while deserializing Quantity");
            }

            var result = Quantity.Create(numericValue);
            if (!result.IsSuccessful)
            {
                // Bubble up the domain validation error as a JSON error so callers get a clear message.
                throw new JsonSerializationException($"Invalid Quantity value while deserializing: {result.Error_msg}");
            }

            return result.Value;
        }
    }
}
