using System.Text.Json;
using RestSharp;
using RominaBackend.Types;

namespace RominaBackend.API.Score;

public class ScoreClient
{
    private readonly RestClient _client;

    public ScoreClient(RestClient client)
    {
        _client = client;
    }

    private bool IsTokenInHeaders()
    {
        var authorizationToken = _client.DefaultParameters.FirstOrDefault(p => p.Name == "Authorization" && p.Type == ParameterType.HttpHeader);

        return authorizationToken != null;
    }

    public async Task<ScoreResponse> AddScore(string game, long score)
    {
        if (!IsTokenInHeaders())
        {
            throw new Exception("Error: No token provided.");
        }

        RestRequest request = new("score", Method.Post);
        request.AddJsonBody(new { game, score });

        RestResponse response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return DeserializeResponse<ScoreResponse>(response.Content)
                ?? throw new Exception("Failed to deserialize profile data.");
        }
        else
        {
            throw HandleErrorResponse(response);
        }
    }

    private static T? DeserializeResponse<T>(string? content)
    {
        if (string.IsNullOrEmpty(content))
        {
            throw new Exception("Empty response content received.");
        }

        try
        {
            return JsonSerializer.Deserialize<T>(content);
        }
        catch (JsonException)
        {
            throw new Exception($"Error while deserializing the response to {typeof(T).Name}.");
        }
    }

    private static Exception HandleErrorResponse(RestResponse response)
    {
        if (string.IsNullOrEmpty(response.Content))
        {
            return new Exception("Empty error content received.");
        }

        try
        {
            var err = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
            return err != null
                ? new Exception($"{err.Error}: {err.Message}")
                : new Exception("Unknown error occurred.");
        }
        catch (JsonException)
        {
            return new Exception("Error while deserializing the error response.");
        }
    }
}
