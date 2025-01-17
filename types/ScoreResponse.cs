using System.Text.Json.Serialization;

namespace RominaBackend.Types;

public record ScoreResponse
{
    [JsonPropertyName("_id")]
    public string? ScoreId { get; init; }
    
    [JsonPropertyName("user")]
    public string? User { get; init; }

    [JsonPropertyName("game")]
    public string? Game { get; init; }

    [JsonPropertyName("score")]
    public long? Score { get; init; }

}