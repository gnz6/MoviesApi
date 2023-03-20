using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController : CustomBaseController
    {
        //private readonly MoviesApiDbContext context;
        //private readonly IMapper mapper;

        public GenreController(MoviesApiDbContext context, IMapper mapper) : base(context, mapper)
        {
            //this.context = context;
            //this.mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            //var entities = await context.Genres.ToListAsync();
            //var dtos = mapper.Map<List<GenreDTO>>(entities);
            //return dtos;

            return await Get<Genre, GenreDTO>();

        }


        [HttpGet("{id:int}", Name = "GetOneGenre")]
        public async Task<ActionResult<GenreDTO>> GetOneGenre(int id)
        {
            //var entity = await context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            //if (entity == null)
            //{
            //    return NotFound();
            //}

            //var dto = mapper.Map<GenreDTO>(entity);
            //return dto;

            return await Get<Genre, GenreDTO>(id); 

        }

        [HttpPost]
        public async Task<ActionResult<GenreDTO>> CreateGenre([FromBody] GenreCreationDTO NewGenre)
        {
            //var entity = mapper.Map<Genre>(NewGenre);
            //context.Add(entity);
            //await context.SaveChangesAsync();
            //var genreDTO = mapper.Map<GenreDTO>(entity);

            //return new CreatedAtRouteResult("GetOneGenre", new { id = genreDTO.Id }, genreDTO);
            return await Post<GenreCreationDTO, Genre, GenreDTO>(NewGenre, "GetOneGenre");
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult<GenreDTO>> EditGenre(int id, [FromBody] GenreCreationDTO EditedGenre)
        {
            //var entity = mapper.Map<Genre>(EditedGenre); 
            //entity.Id = id; 
            //context.Entry(entity).State = EntityState.Modified;
            //await context.SaveChangesAsync();
            //return NoContent();

            return await Put<GenreCreationDTO, Genre>(id, EditedGenre);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteGenre(int id)
        {
            //var exist = await context.Genres.AnyAsync(g => g.Id == id);
            //if (!exist)
            //{
            //    return NotFound();
            //}
            //context.Remove(new Genre() { Id = id });
            //await context.SaveChangesAsync(); 
            //return NoContent();

            return await Delete<Genre>(id);
        }
    
    }
}
