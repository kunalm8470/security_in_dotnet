using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models.Requests
{
    public class RefreshTokenDto
    {
        [JsonPropertyName("grant_type")]
        [Required]
        [RegularExpression("refresh_token", ErrorMessage = @"Value MUST be set to ""refresh_token""")]
        public string GrantType { get; set; }

        [JsonPropertyName("refresh_token")]
        [Required]
        public string RefreshToken { get; set; }
    }
}
