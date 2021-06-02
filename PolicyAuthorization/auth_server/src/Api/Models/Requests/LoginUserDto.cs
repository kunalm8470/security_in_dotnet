using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models.Requests
{
    public class LoginUserDto
    {
        [JsonPropertyName("grant_type")]
        [Required]
        [RegularExpression("password", ErrorMessage = @"Value MUST be set to ""password""")]
        public string GrantType { get; set; }

        [JsonPropertyName("username")]
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        [Required]
        public string Password { get; set; }
    }
}
