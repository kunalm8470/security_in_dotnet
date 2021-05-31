using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class LoginResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; }
        
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; }

        public LoginResponse(string accessToken,
            int expiresIn,
            string refreshToken)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }
    }
}
