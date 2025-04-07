namespace MoviesToWatch.Server.Models
{
    public class PagedResponse<T>
    {
        public int Page { get; set; }
        public required IEnumerable<T> Results { get; set; }
    }
}
