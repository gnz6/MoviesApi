using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorController : ControllerBase
    {
        private readonly MoviesApiDbContext context;
        private readonly IMapper mapper;

        public ActorController(MoviesApiDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<ActorDTO>> GetActors()
        {
            var entities = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(entities);
        }

        [HttpGet("{id: int}", Name = "GetOneActor")]
        public async Task<ActionResult<ActorDTO>>GetOneActor(int id)
        {
            var entity = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if( entity == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<ActorDTO>(entity);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> CreateActor([FromBody]ActorCreationDTO NewActor)
        {
            var entity = mapper.Map<Actor>(NewActor);
            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entity);
            return new CreatedAtRouteResult("GetOneActor", new { id = entity.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditActor (int id, [FromBody]ActorCreationDTO EditedActor)
        {
            var entity = mapper.Map<Actor>(EditedActor);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();   
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteActor(int id)
        {
            var exist = await context.Actors.AnyAsync(g => g.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
