using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models.Requests
{
    public class RevokeTokenDto
    {
        [JsonPropertyName("token")]
        [Required]
        public string Token { get; set; }

        [JsonPropertyName("token_type_hint")]
        [Required]
        [RegularExpression("refresh_token", ErrorMessage = @"Value MUST be set to ""refresh_token""")]
        public string TokenTypeHint { get; set; }
    }
}
