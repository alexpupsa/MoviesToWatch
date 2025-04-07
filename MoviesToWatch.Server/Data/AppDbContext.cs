using Microsoft.EntityFrameworkCore;
using MoviesToWatch.Server.Data.Entities;

namespace MoviesToWatch.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<MovieComment> MovieComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieComment>().ToTable("MovieComments");
        }
    }

}
