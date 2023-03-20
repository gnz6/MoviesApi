using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
    public class MoviesApiDbContext : IdentityDbContext
    {
        public MoviesApiDbContext(DbContextOptions<MoviesApiDbContext> options) : base(options) 
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movies_Actors>().HasKey(x => new { x.MovieId, x.ActorId });
            modelBuilder.Entity<Movies_Genres>().HasKey(x => new { x.MovieId, x.GenreId });
            modelBuilder.Entity<Movies_Rooms>().HasKey(x => new { x.MovieId, x.RoomId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Movies_Actors> Movies_Actors { get; set; }
        public DbSet<Movies_Genres> Movies_Genres { get; set; }
        public DbSet<Movies_Rooms> Movies_Rooms { get; set; }
    }
}
