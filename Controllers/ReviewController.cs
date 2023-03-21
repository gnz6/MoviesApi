using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Migrations;
using MoviesApi.Models;
using System.Security.Claims;

namespace MoviesApi.Controllers
{

    [ApiController]
    [Route("api/movies/{movieId:int}/review")]
    public class ReviewController : CustomBaseController
    {
        private readonly MoviesApiDbContext context;
        private readonly IMapper mapper;

        public ReviewController(MoviesApiDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReviewDTO>>>Get(int movieId, [FromQuery]PagingDTO pagingDTO)
        {
            var queryable = context.Reviews.Include(x => x.User.Id).AsQueryable();
            queryable = queryable.Where(x => x.MovieId == movieId);
            return await Get<Review, ReviewDTO>(pagingDTO, queryable);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult > Post(int movieId, [FromBody] ReviewCreateDTO reviewCreateDTO)
        {

            var movieExists = await context.Movies.AnyAsync(x => x.Id == movieId);

            if (!movieExists)
            {
                return NotFound();
            }
           

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var reviewExists = await context.Reviews.AnyAsync(x => x.MovieId == movieId && x.UserId == Convert.ToInt32(userId));

            if (reviewExists) 
            {
                var review = mapper.Map<Review>(reviewCreateDTO);
                review.MovieId = movieId;
                review.UserId = Convert.ToInt32(userId);

                context.Add(review);
                await context.SaveChangesAsync();
                return NoContent();

            }
            return NoContent();
        }

        [HttpPut("{reviewId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int reviewId,int movieId, [FromBody] ReviewCreateDTO reviewCreateDTO, Review reviewDb)
        {
            var movieExists = await context.Movies.AnyAsync(x => x.Id == movieId);

            if (!movieExists)
            {
                return NotFound();
            }

            var review = await context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);

            if (review == null)
            {
                return NotFound();
            }

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if(review.Id != Convert.ToInt32(userId)) { return Forbid(); }

            //reviewDb = mapper.Map<Review>(reviewCreateDTO, reviewDbreviewDb);
            review = mapper.Map<Review>(reviewCreateDTO);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        public async Task<ActionResult> Delete(int reviewId)
        {
            var review = await context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);

            if (review == null)
            {
                return NotFound();
            }
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (review.Id != Convert.ToInt32(userId)) { return Forbid(); }
            context.Remove(review);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
