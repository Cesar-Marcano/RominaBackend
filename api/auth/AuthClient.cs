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
                var existingHeader = _client.DefaultParameters
                    .FirstOrDefault(p => p.Name == "Authorization" && p.Type == ParameterType.HttpHeader);

                if (existingHeader != null)
                {
                    _client.DefaultParameters.RemoveParameter("Authorization", ParameterType.HttpHeader);
                }

                _token = value;

                _client.AddDefaultHeader("Authorization", $"Bearer {value}");
            }
        }
    }

    public AuthClient(RestClient client, string? token = null)
    {
        _client = client;
        Token = token;
    }

    public async Task<string> SignUp(string username, string password)
    {
        RestRequest request = new("auth/register", Method.Post);
        request.AddJsonBody(new { username, password });

        RestResponse response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            if (string.IsNullOrEmpty(response.Content))
            {
                throw new Exception("Empty response content received.");
            }

            try
            {
                var responseData = JsonSerializer.Deserialize<AuthResponse>(response.Content);
                if (responseData == null || string.IsNullOrEmpty(responseData.AccessToken))
                {
                    throw new Exception("The client didn't receive a valid token.");
                }

                Token = responseData.AccessToken;
                return Token;
            }
            catch (JsonException)
            {
                throw new Exception("Error while deserializing the response.");
            }
        }
        else
        {
            try
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception("Empty error content received.");
                }

                var err = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
                if (err != null)
                {
                    throw new Exception($"{err.Error}: {err.Message}");
                }
                else
                {
                    throw new Exception("Unknown error occurred.");
                }
            }
            catch (JsonException)
            {
                throw new Exception("Error while deserializing the error response.");
            }
        }
    }

    public async Task<string> SignIn(string username, string password)
    {
        RestRequest request = new("auth/login", Method.Post);
        request.AddJsonBody(new { username, password });

        RestResponse response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            if (string.IsNullOrEmpty(response.Content))
            {
                throw new Exception("Empty response content received.");
            }

            try
            {
                var responseData = JsonSerializer.Deserialize<AuthResponse>(response.Content);
                if (responseData == null || string.IsNullOrEmpty(responseData.AccessToken))
                {
                    throw new Exception("The client didn't receive a valid token.");
                }

                Token = responseData.AccessToken;
                return Token;
            }
            catch (JsonException)
            {
                throw new Exception("Error while deserializing the response.");
            }
        }
        else
        {
            try
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception("Empty error content received.");
                }

                var err = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
                if (err != null)
                {
                    throw new Exception($"{err.Error}: {err.Message}");
                }
                else
                {
                    throw new Exception("Unknown error occurred.");
                }
            }
            catch (JsonException)
            {
                throw new Exception("Error while deserializing the error response.");
            }
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
            if (string.IsNullOrEmpty(response.Content))
            {
                throw new Exception("Empty response content received.");
            }

            try
            {
                var responseData = JsonSerializer.Deserialize<ProfileResponse>(response.Content);

                if (responseData == null)
                {
                    throw new Exception("Failed to deserialize profile data.");
                }

                return responseData;
            }
            catch (JsonException)
            {
                throw new Exception("Error while deserializing the response.");
            }
        }
        else
        {
            try
            {
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception("Empty error content received.");
                }

                var err = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
                if (err != null)
                {
                    throw new Exception($"{err.Error}: {err.Message}");
                }
                else
                {
                    throw new Exception("Unknown error occurred.");
                }
            }
            catch (JsonException)
            {
                throw new Exception("Error while deserializing the error response.");
            }
        }
    }

}