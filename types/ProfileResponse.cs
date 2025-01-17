using System.Text.Json.Serialization;

namespace RominaBackend.Types;

public record ProfileResponse
{
    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("id")]
    public string? UserId { get; init; }

    [JsonPropertyName("tokenIssuedAt")]
    public long? TokenIssued { get; init; }

    [JsonPropertyName("tokenExpirationTime")]
    public long? TokenExpiration { get; init; }
}