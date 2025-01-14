using System.Text.Json.Serialization;

namespace RominaBackend.types;

record AuthResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }
}