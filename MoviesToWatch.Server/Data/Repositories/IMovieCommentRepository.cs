using MoviesToWatch.Server.Data.Entities;

namespace MoviesToWatch.Server.Data.Repositories
{
    public interface IMovieCommentRepository
    {
        Task<List<MovieComment>> GetCommentsByMovieIdAsync(int movieId);
        Task<MovieComment> AddCommentAsync(MovieComment comment);
    }
}
