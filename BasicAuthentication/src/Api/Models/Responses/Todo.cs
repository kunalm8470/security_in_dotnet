using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class Todo
    {
        [JsonPropertyName("id")]
        public int Id { get; }

        [JsonPropertyName("title")]
        public string Title { get; }

        [JsonPropertyName("completed")]
        public bool Completed { get; }

        public Todo(int id,
            string title,
            bool completed)
        {
            Id = id;
            Title = title;
            Completed = completed;
        }
    }
}
