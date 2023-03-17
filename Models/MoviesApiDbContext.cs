﻿using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
    public class MoviesApiDbContext : DbContext
    {
        public MoviesApiDbContext(DbContextOptions<MoviesApiDbContext> options) : base(options) 
        { 

        }  
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
