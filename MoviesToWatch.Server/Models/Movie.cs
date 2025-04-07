namespace MoviesToWatch.Server.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Overview { get; set; }

        public string? PosterPath { get; set; }

        public string? ReleaseDate { get; set; }

        public List<string> Genres { get; set; } = new List<string>();

        public List<string> Actors { get; set; } = new List<string>();

        public List<string> Images { get; set; } = new List<string>();
    }
}
