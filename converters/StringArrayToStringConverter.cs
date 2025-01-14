using System.Text.Json;
using System.Text.Json.Serialization;

namespace RominaBackend.converters;
public class StringArrayToStringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString()!;
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            var array = JsonSerializer.Deserialize<string[]>(ref reader, options);
            return array != null ? string.Join(", ", array) : string.Empty;
        }

        throw new JsonException($"Unexpected token type: {reader.TokenType}");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
