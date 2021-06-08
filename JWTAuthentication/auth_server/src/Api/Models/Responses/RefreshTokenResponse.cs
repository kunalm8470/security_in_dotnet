using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class RefreshTokenResponse : AccessTokenResponse
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; }

        public RefreshTokenResponse(string accessToken, int expiresIn, string refreshToken) : base(accessToken, expiresIn)
        {
            RefreshToken = refreshToken;
        }
    }
}
