using MoviesToWatch.Server.Models;

namespace MoviesToWatch.Server.Services.Interfaces
{
    public interface IMovieDBApiService
    {
        Task<IEnumerable<Movie>?> GetTopRatedMoviesAsync();
        Task<IEnumerable<Movie>?> GetLatestMovies();
        Task<IEnumerable<Movie>?> SearchMovies(string searchType, string keyword);
        Task<Movie?> GetMovieDetails(int movieId);
    }
}
