using AutoMapper;
using MoviesToWatch.Server.Models;
using MoviesToWatch.Server.Services.Interfaces;
using Newtonsoft.Json;

namespace MoviesToWatch.Server.Services
{
    public class MovieDBApiService : IMovieDBApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public MovieDBApiService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Movie>?> GetTopRatedMoviesAsync()
        {
            var topRatedResponse = await _httpClient.GetAsync("movie/top_rated");
            var topRated = await topRatedResponse.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<PagedResponse<DBApiMovie>>(topRated);

            return movies?.Results.Select(x => _mapper.Map<Movie>(x));
        }

        public async Task<IEnumerable<Movie>?> GetLatestMovies()
        {
            var latestResponse = await _httpClient.GetAsync("movie/now_playing");
            var latest = await latestResponse.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<PagedResponse<DBApiMovie>>(latest);

            return movies?.Results.Select(x => _mapper.Map<Movie>(x));
        }

        public async Task<IEnumerable<Movie>?> SearchMovies(string searchType, string keyword)
        {
            var searchUrl = searchType switch
            {
                "title" => $"search/movie?query={keyword}",
                "genre" => $"discover/movie?with_genres={keyword}",
                _ => null
            };

            if (searchUrl != null)
            {
                var response = await _httpClient.GetStringAsync(searchUrl);
                var movies = JsonConvert.DeserializeObject<PagedResponse<DBApiMovie>>(response);

                return movies?.Results.Select(x => _mapper.Map<Movie>(x));
            }

            return null;
        }

        public async Task<Movie?> GetMovieDetails(int movieId)
        {
            var detailsTask = _httpClient.GetStringAsync($"movie/{movieId}");
            var creditsTask = _httpClient.GetStringAsync($"movie/{movieId}/credits");
            var imagesTask = _httpClient.GetStringAsync($"movie/{movieId}/images?language=en");

            await Task.WhenAll(detailsTask, creditsTask, imagesTask);

            var detailsResponse = detailsTask.Result;
            var creditsResponse = creditsTask.Result;
            var imagesResponse = imagesTask.Result;

            var movieDetails = JsonConvert.DeserializeObject<DBApiMovie>(detailsResponse);
            if (movieDetails != null)
            {
                var movie = _mapper.Map<Movie>(movieDetails);
                var credits = JsonConvert.DeserializeObject<CreditsResponse>(creditsResponse);

                if (credits?.Cast != null)
                {
                    movie.Actors = credits.Cast.Take(10).Select(x => x.Name).ToList();
                }

                var images = JsonConvert.DeserializeObject<ImageResponse>(imagesResponse);

                if (images?.Posters != null)
                {
                    movie.Images = images.Posters.Take(10).Select(x => x.FilePath).ToList();
                }

                return movie;
            }

            return null;
        }
    }

    public class CreditsResponse
    {
        public required List<Actor> Cast { get; set; }
    }

    public class Actor
    {
        public required string Name { get; set; }
    }

    public class ImageResponse
    {
        public required List<Image> Posters { get; set; }
    }

    public class Image
    {
        [JsonProperty(PropertyName = "file_path")]
        public required string FilePath { get; set; }
    }
}
