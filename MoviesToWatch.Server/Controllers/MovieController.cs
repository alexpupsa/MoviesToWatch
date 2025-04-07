using Microsoft.AspNetCore.Mvc;
using MoviesToWatch.Server.Services.Interfaces;

namespace MoviesToWatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieDBApiService _movieDBApiService;

        public MovieController(IMovieDBApiService movieDBApiService)
        {
            _movieDBApiService = movieDBApiService;
        }

        [HttpGet("top-rated")]
        public async Task<IActionResult> GetTopRated()
        {
            var movies = await _movieDBApiService.GetTopRatedMoviesAsync();
            if (movies == null)
            {
                return NotFound("No latest movies found.");
            }
            return Ok(movies);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var movies = await _movieDBApiService.GetLatestMovies();
            if (movies == null)
            {
                return NotFound("No latest movies found.");
            }
            return Ok(movies);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetMovieDetails(int id)
        {
            var movie = await _movieDBApiService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }
            return Ok(movie);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies([FromQuery] string searchType, [FromQuery] string keyword)
        {
            var movies = await _movieDBApiService.SearchMovies(searchType, keyword);
            if (movies == null)
            {
                return NotFound($"Search returned no results.");
            }
            return Ok(movies);
        }
    }
}
