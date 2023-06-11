using System.Text.Json.Serialization;

namespace _6_5ApiHomework.Data
{
    public class Joke

    {
        public int Id { get; set; }

        [JsonPropertyName("jokeId")]
        public int OriginalId { get; set; }

        [JsonPropertyName("setup")]
        public string Question { get; set; }

        [JsonPropertyName("punchline")]
        public string Answer { get; set; }

        public InteractionStatus InteractionStatus { get; set; }


        public List<UserLikedJokes> UserLikedJokes { get; set; }
    }
}