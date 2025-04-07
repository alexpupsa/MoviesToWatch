using Microsoft.EntityFrameworkCore;
using MoviesToWatch.Server.Data.Entities;

namespace MoviesToWatch.Server.Data.Repositories
{
    public class MovieCommentRepository : IMovieCommentRepository
    {
        private readonly AppDbContext _context;

        public MovieCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieComment>> GetCommentsByMovieIdAsync(int movieId)
        {
            return await _context.MovieComments
                .Where(c => c.MovieId == movieId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<MovieComment> AddCommentAsync(MovieComment comment)
        {
            _context.MovieComments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }

}
