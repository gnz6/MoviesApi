using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Models;
using MoviesApi.Services;
using System.ComponentModel;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private readonly MoviesApiDbContext context;

        public readonly IMapper mapper;
        public readonly IStorage storage;
        private readonly string container = "movies";


        public MovieController( MoviesApiDbContext context, IMapper mapper, IStorage storage)
        {
            this.context = context;
            this.mapper = mapper;
            this.storage = storage;

        }

        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> GetMovies()
        {
            var movies = await context.Movies.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id}" , Name = "GetOneMovie")]
        public async Task<ActionResult<MovieDTO>> GetOneMovie(int id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<MovieDTO>(movie);

        }


        [HttpPost]
        public async Task<ActionResult> CreateMovie([FromForm]MovieCreationDTO NewMovie)
        {
            var movie = mapper.Map<Movie>(NewMovie);
            if (NewMovie.Poster != null)
            {
                using (var stream = new MemoryStream())
                {
                    await NewMovie.Poster.CopyToAsync(stream);
                    var content = stream.ToArray();
                    var extension = Path.GetExtension(NewMovie.Poster.FileName);
                    movie.Poster = await storage.StoreFile(content, extension, container, NewMovie.Poster.ContentType);
                }
            }

            context.Add(movie);
            await context.SaveChangesAsync();
            var dto = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("GetOneMovie", new { id = movie.Id }, dto);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> EditMovie([FromForm]  MovieCreationDTO EditedMovie, int id)
        {
            var movieDB = await context.Movies.FirstOrDefaultAsync(a => a.Id == id);
            if (movieDB == null) { return NotFound(); }

            movieDB = mapper.Map(EditedMovie, movieDB);

            if (EditedMovie.Poster != null)
            {
                using (var stream = new MemoryStream())
                {
                    await EditedMovie.Poster.CopyToAsync(stream);
                    var content = stream.ToArray();
                    var extension = Path.GetExtension(EditedMovie.Poster.FileName);
                    movieDB.Poster = await storage.EditFile(content, extension, container, movieDB.Poster, EditedMovie.Poster.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> EditField(int id, [FromBody] JsonPatchDocument<PatchMovieDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entityDB = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (entityDB == null) { return NotFound(); }

            var entityDTO = mapper.Map<PatchMovieDTO>(entityDB);

            patchDocument.ApplyTo(entityDTO, ModelState);

            var isValid = TryValidateModel(entityDTO);

            if (!isValid)

            {
                return BadRequest(ModelState);
            }

            mapper.Map(entityDTO, entityDB);

            await context.SaveChangesAsync();

            return NoContent();


        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var exist = await context.Movies.AnyAsync(g => g.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Movie() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
    }

