using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class RefreshTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; }

        public RefreshTokenResponse(string accessToken, int expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }
    }
}
