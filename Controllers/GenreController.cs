using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController
    {
        private readonly MoviesApiDbContext context;
        private readonly IMapper mapper;

        public GenreController( MoviesApiDbContext context , IMapper mapper) 
        {
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            var entities = await context.Genres.ToListAsync();
            var dtos = mapper.Map<List<GenreDTO>>(entities);
            return dtos;
        }
    }
}
