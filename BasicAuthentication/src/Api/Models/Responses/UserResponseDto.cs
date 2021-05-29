using System;
using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class UserResponseDto
    {
        [JsonPropertyName("user_id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("gender_abbreviation")]
        public char GenderAbbreviation { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
