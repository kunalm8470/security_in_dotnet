using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class AccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; }

        public AccessTokenResponse(string accessToken, int expiresIn) : base()
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }
    }
}
