using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Helpers;
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


        public MovieController(MoviesApiDbContext context, IMapper mapper, IStorage storage)
        {
            this.context = context;
            this.mapper = mapper;
            this.storage = storage;

        }

        [HttpGet]
        public async Task<ActionResult<MovieIndexDTO>> GetMovies() 
        {
            var top = 5;
            var today = DateTime.Today;

            var nextRelease = await context.Movies
                .Where(x => x.ReleaseDate > today)
                .OrderBy(x => x.ReleaseDate)
                .Take(top).ToListAsync();

            var inCinema = await context.Movies
                .Where(x => x.InCinema == true)
                .Take(top).ToListAsync();

            var result  = new MovieIndexDTO();
            result.NextReleases = mapper.Map<List<MovieDTO>>(nextRelease);
            result.InCinemas = mapper.Map<List<MovieDTO>>(inCinema);
            return result;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> GetMoviesBy([FromQuery] FilterMovieDTO filterMovieDTO )
        {
            var today = DateTime.Today;

            var movieQueryable = context.Movies.AsQueryable();
            
            if(!string.IsNullOrEmpty(filterMovieDTO.Title))
            {
                movieQueryable = movieQueryable.Where( x => x.Title.Contains(filterMovieDTO.Title) );
            }

            if (filterMovieDTO.InCinema)
            {
                movieQueryable = movieQueryable.Where(x => x.InCinema);
            }

            if (filterMovieDTO.NextRelease)
            {
                movieQueryable = movieQueryable.Where(x => x.ReleaseDate > today);
            }

            //Filtrar por genero

            if (filterMovieDTO.GenreId != 0)
            {
                movieQueryable = movieQueryable.Where(x => x.Movies_Genres.Select(y => y.GenreId).Contains(filterMovieDTO.GenreId));
            }

            await HttpContext.InsertParams(movieQueryable, filterMovieDTO.ItemsPerPage);

            var movies = await movieQueryable.Paginate(filterMovieDTO.Paging).ToListAsync();

            return mapper.Map<List<MovieDTO>>( movies );

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
            AssignActorOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            var dto = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("GetOneMovie", new { id = movie.Id }, dto);
        }

        private void AssignActorOrder(Movie movie)
        {
            if (movie != null)
            {
                for ( int i = 0; i< movie.Movies_Actors.Count; i++ )
                {
                    movie.Movies_Actors[i].Order = i;
                }
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> EditMovie([FromForm]  MovieCreationDTO EditedMovie, int id)
        {
            var movieDB = await context.Movies
                .Include(x=> x.Movies_Actors)
                .Include(x=> x.Movies_Genres)
                .FirstOrDefaultAsync(a => a.Id == id);
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
            AssignActorOrder(movieDB);
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

