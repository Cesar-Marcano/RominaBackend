using System.Text.Json;
using System.Text.Json.Serialization;
using RominaBackend.converters;

namespace RominaBackend.types;
record ErrorResponse
{
    [JsonPropertyName("message")]
    [JsonConverter(typeof(StringArrayToStringConverter))]
    public string? Message { get; init; }

    [JsonPropertyName("error")]
    public string? Error { get; init; }

    [JsonPropertyName("statusCode")]
    public int? StatusCode { get; init; }
}