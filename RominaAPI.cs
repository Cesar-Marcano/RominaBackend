using RestSharp;
using RominaBackend.API.Auth;
using RominaBackend.API.Score;

namespace RominaBackend;
public class RominaAPI
{
    private readonly RestClient _client;
    private readonly ScoreClient? _scoreClient;

    public readonly AuthClient authClient;

    public RominaAPI(string baseUrl, string? token = null)
    {
        _client = new RestClient(baseUrl);

        if (token != null)
        {
            _client.AddDefaultHeader("Authorization", $"Bearer ${token}");

            _scoreClient = new ScoreClient(_client);
        }

        authClient = new AuthClient(_client, token);
    }

    public ScoreClient GetScoreClient()
    {
        return _scoreClient ?? throw new Exception("Internal Error: ScoreClient is not initialized.");
    }
}
