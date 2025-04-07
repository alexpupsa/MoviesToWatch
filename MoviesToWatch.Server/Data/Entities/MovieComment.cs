namespace MoviesToWatch.Server.Data.Entities
{
    public class MovieComment
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public required string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
