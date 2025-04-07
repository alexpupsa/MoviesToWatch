using AutoMapper;
using Moq;
using MoviesToWatch.Server.Models;
using MoviesToWatch.Server.Services;
using MoviesToWatch.UnitTests.Helpers;
using Newtonsoft.Json;

namespace MoviesToWatch.UnitTests.Services
{
    [TestFixture]
    public class MovieDBApiServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private MovieDBApiService _service;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://address.mock/");

            _mapperMock = new Mock<IMapper>();

            _service = new MovieDBApiService(httpClient, _mapperMock.Object);
        }

        [Test]
        public async Task GetTopRatedMoviesAsync_ShouldReturnMappedMovies_WhenResponseIsSuccessful()
        {
            // Arrange
            var response = new PagedResponse<DBApiMovie>
            {
                Results = new List<DBApiMovie>
                {
                    new DBApiMovie { Id = 1, Title = "Movie 1", Overview = "This is a movie about nothing" },
                    new DBApiMovie { Id = 2, Title = "Movie 2", Overview = "This is a movie about nothing" }
                }
            };
            var jsonResponse = JsonConvert.SerializeObject(response);
            _mockHttpMessageHandler.SetupRequest(HttpMethod.Get, "movie/top_rated",
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mapperMock.Setup(m => m.Map<Movie>(It.IsAny<DBApiMovie>()))
                .Returns((DBApiMovie dbMovie) => new Movie { Id = dbMovie.Id, Title = dbMovie.Title, Overview = dbMovie.Overview });

            // Act
            var result = await _service.GetTopRatedMoviesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo("Movie 1"));
        }

        [Test]
        public async Task GetMovieDetails_ShouldReturnMappedMovie_WhenResponseIsSuccessful()
        {
            // Arrange
            var movieResponse = new DBApiMovie { Id = 1, Title = "Movie 1", Overview = "This is a movie about nothing" };
            var creditsResponse = new CreditsResponse { Cast = new List<Actor> { new Actor { Name = "Actor 1" } } };
            var imagesResponse = new ImageResponse { Posters = new List<Image> { new Image { FilePath = "/path/to/image" } } };

            var movieJson = JsonConvert.SerializeObject(movieResponse);
            var creditsJson = JsonConvert.SerializeObject(creditsResponse);
            var imagesJson = JsonConvert.SerializeObject(imagesResponse);

            _mockHttpMessageHandler.SetupRequest(HttpMethod.Get, "movie/1",
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(movieJson)
                });

            _mockHttpMessageHandler.SetupRequest(HttpMethod.Get, "movie/1/credits",
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(creditsJson)
                });

            _mockHttpMessageHandler.SetupRequest(HttpMethod.Get, "movie/1/images?language=en",
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(imagesJson)
                });

            _mapperMock.Setup(m => m.Map<Movie>(It.IsAny<DBApiMovie>()))
                .Returns((DBApiMovie dbMovie) => new Movie { Id = dbMovie.Id, Title = dbMovie.Title, Overview = dbMovie.Overview });

            // Act
            var result = await _service.GetMovieDetails(1);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Title, Is.EqualTo("Movie 1"));
            Assert.That(result.Actors.First(), Is.EqualTo("Actor 1"));
            Assert.That(result.Images.First(), Is.EqualTo("/path/to/image"));
        }

        [Test]
        public async Task GetLatestMovies_ShouldReturnMappedMovies_WhenResponseIsSuccessful()
        {
            // Arrange
            var response = new PagedResponse<DBApiMovie>
            {
                Results = new List<DBApiMovie>
                {
                    new DBApiMovie { Id = 1, Title = "Latest Movie 1", Overview = "This is a movie about nothing" },
                    new DBApiMovie { Id = 2, Title = "Latest Movie 2", Overview = "This is a movie about nothing" }
                }
            };
            var jsonResponse = JsonConvert.SerializeObject(response);
            _mockHttpMessageHandler.SetupRequest(HttpMethod.Get, "movie/now_playing",
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mapperMock.Setup(m => m.Map<Movie>(It.IsAny<DBApiMovie>()))
                .Returns((DBApiMovie dbMovie) => new Movie { Id = dbMovie.Id, Title = dbMovie.Title, Overview = dbMovie.Overview });

            // Act
            var result = await _service.GetLatestMovies();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo("Latest Movie 1"));
        }

        [Test]
        public async Task SearchMovies_ShouldReturnMappedMovies_WhenSearchTypeIsTitle()
        {
            // Arrange
            var response = new PagedResponse<DBApiMovie>
            {
                Results = new List<DBApiMovie>
                {
                    new DBApiMovie { Id = 1, Title = "Search Movie 1", Overview = "This is a movie about nothing" },
                    new DBApiMovie { Id = 2, Title = "Search Movie 2", Overview = "This is a movie about nothing" }
                }
            };
            var jsonResponse = JsonConvert.SerializeObject(response);
            _mockHttpMessageHandler.SetupRequest(HttpMethod.Get, "search/movie?query=keyword",
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Arrange: Mock the AutoMapper mapping
            _mapperMock.Setup(m => m.Map<Movie>(It.IsAny<DBApiMovie>()))
                .Returns((DBApiMovie dbMovie) => new Movie { Id = dbMovie.Id, Title = dbMovie.Title, Overview = dbMovie.Overview });

            // Act
            var result = await _service.SearchMovies("title", "keyword");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo("Search Movie 1"));
        }

        [Test]
        public async Task SearchMovies_ShouldReturnMappedMovies_WhenSearchTypeIsGenre()
        {
            // Arrange: 
            var response = new PagedResponse<DBApiMovie>
            {
                Results = new List<DBApiMovie>
                {
                    new DBApiMovie { Id = 1, Title = "Genre Movie 1", Overview = "This is a movie about nothing" },
                    new DBApiMovie { Id = 2, Title = "Genre Movie 2", Overview = "This is a movie about nothing" }
                }
            };
            var jsonResponse = JsonConvert.SerializeObject(response);
            _mockHttpMessageHandler.SetupRequest(HttpMethod.Get, "discover/movie?with_genres=keyword",
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mapperMock.Setup(m => m.Map<Movie>(It.IsAny<DBApiMovie>()))
                .Returns((DBApiMovie dbMovie) => new Movie { Id = dbMovie.Id, Title = dbMovie.Title, Overview = dbMovie.Overview });

            // Act:
            var result = await _service.SearchMovies("genre", "keyword");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo("Genre Movie 1"));
        }
    }
}
