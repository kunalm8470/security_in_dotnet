using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class AuthenticationConfiguration
    {
        [JsonPropertyName("AccessTokenExpirationMinutes")]
        public int AccessTokenExpirationMinutes { get; set; }

        [JsonPropertyName("RefreshTokenExpirationMinutes")]
        public int RefreshTokenExpirationMinutes { get; set; }

        [JsonPropertyName("Issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("Audience")]
        public string Audience { get; set; }
        public AsymmetricKeys AccessTokenKeys { get; set; }
        public AsymmetricKeys RefreshTokenKeys { get; set; }
    }

    public class AsymmetricKeys
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
