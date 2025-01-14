using System.Text.Json.Serialization;

namespace RominaBackend.types;

public record ProfileResponse
{
    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("sub")]
    public string? UserId { get; init; }

    [JsonPropertyName("iat")]
    public long? TokenCreated { get; init; }

    [JsonPropertyName("exp")]
    public long? TokenExpires { get; init; }
}