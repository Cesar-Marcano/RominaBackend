using System.Text.Json;
using RestSharp;
using RominaBackend.types;

namespace RominaBackend.api.auth;

public class AuthClient
{
    private readonly RestClient _client;
    private string? _token;

    public string? Token
    {
        get => _token;
        private set
        {
            if (_token != value)
            {
                UpdateAuthorizationHeader(value);
                _token = value;
            }
        }
    }

    public AuthClient(RestClient client, string? token = null)
    {
        _client = client;
        Token = token;
    }

    private void UpdateAuthorizationHeader(string? token)
    {
        var existingHeader = _client.DefaultParameters
            .FirstOrDefault(p => p.Name == "Authorization" && p.Type == ParameterType.HttpHeader);

        if (existingHeader != null)
        {
            _client.DefaultParameters.RemoveParameter("Authorization", ParameterType.HttpHeader);
        }

        if (!string.IsNullOrEmpty(token))
        {
            _client.AddDefaultHeader("Authorization", $"Bearer {token}");
        }
    }

    public async Task<string> SignUp(string username, string password) =>
        await HandleAuthRequest("auth/register", username, password);

    public async Task<string> SignIn(string username, string password) =>
        await HandleAuthRequest("auth/login", username, password);

    private async Task<string> HandleAuthRequest(string endpoint, string username, string password)
    {
        RestRequest request = new(endpoint, Method.Post);
        request.AddJsonBody(new { username, password });

        RestResponse response = await _client.ExecuteAsync(request);
        return ProcessAuthResponse(response);
    }

    private string ProcessAuthResponse(RestResponse response)
    {
        if (response.IsSuccessful)
        {
            var token = DeserializeResponse<AuthResponse>(response.Content)?.AccessToken;
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Invalid token received.");
            }

            Token = token;
            return Token;
        }
        else
        {
            throw HandleErrorResponse(response);
        }
    }

    public async Task<ProfileResponse> Profile()
    {
        if (Token == null)
        {
            throw new Exception("Error: No token provided.");
        }

        RestRequest request = new("auth/profile", Method.Get);
        RestResponse response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return DeserializeResponse<ProfileResponse>(response.Content)
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
