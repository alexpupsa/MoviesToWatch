using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesToWatch.Server.Data.Entities;
using MoviesToWatch.Server.Data.Repositories;

namespace MoviesToWatch.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IMovieCommentRepository _repository;

        public CommentController(IMovieCommentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetComments(int movieId)
        {
            var comments = await _repository.GetCommentsByMovieIdAsync(movieId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] MovieComment comment)
        {
            var result = await _repository.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetComments), new { movieId = result.MovieId }, result);
        }
    }
}
