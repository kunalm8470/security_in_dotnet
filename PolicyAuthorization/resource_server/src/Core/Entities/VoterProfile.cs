using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class VoterProfile
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; }

        [JsonPropertyName("last_name")]
        public string LastName { get; }

        [JsonPropertyName("age")]
        public int Age { get; }

        [JsonPropertyName("can_vote")]
        public bool CanVote { get; }

        public VoterProfile(string firstName,
            string lastName,
            int age,
            bool canVote)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            CanVote = canVote;
        }
    }
}
