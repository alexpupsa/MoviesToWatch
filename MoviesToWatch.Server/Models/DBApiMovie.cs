using Newtonsoft.Json;

namespace MoviesToWatch.Server.Models
{
    public class DBApiMovie
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Overview { get; set; }

        [JsonProperty(PropertyName = "poster_path")]
        public string? PosterPath { get; set; }

        [JsonProperty(PropertyName = "release_date")]
        public string? ReleaseDate { get; set; }
    }
}
