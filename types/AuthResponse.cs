using System.Text.Json.Serialization;

namespace RominaBackend.Types;

record AuthResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }
}