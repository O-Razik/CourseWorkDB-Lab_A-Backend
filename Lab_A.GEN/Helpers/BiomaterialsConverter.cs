using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Lab_A.GEN.Helpers;

public class BiomaterialsConverter : JsonConverter<string[]>
{
    public override string[]? ReadJson(JsonReader reader, Type objectType, string[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        if (token.Type == JTokenType.String)
        {
            return new string[] { token.ToString() };
        }
        else if (token.Type == JTokenType.Array)
        {
            return token.ToObject<string[]>();
        }
        throw new JsonSerializationException("Unexpected token type: " + token.Type);
    }

    public override void WriteJson(JsonWriter writer, string[]? value, JsonSerializer serializer)
    {
        if (value is { Length: 1 })
        {
            writer.WriteValue(value[0]);
        }
        else
        {
            writer.WriteStartArray();
            if (value != null)
                foreach (var item in value)
                {
                    writer.WriteValue(item);
                }

            writer.WriteEndArray();
        }
    }
}