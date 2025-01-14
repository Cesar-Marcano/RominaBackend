using RestSharp;
using RominaBackend.api.auth;

namespace RominaBackend;
public class RominaAPI
{
    private readonly RestClient _client;
    public readonly AuthClient authClient;

    public RominaAPI(string baseUrl, string? token = null)
    {
        _client = new RestClient(baseUrl);

        if (token != null) {
            _client.AddDefaultHeader("Authorization", $"Bearer ${token}");
        }

        authClient = new AuthClient(_client, token);
    }
}
