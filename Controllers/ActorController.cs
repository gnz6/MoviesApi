using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Helpers;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorController : ControllerBase
    {
        private readonly MoviesApiDbContext context;
        private readonly IMapper mapper;
        private readonly IStorage storage;
        private readonly string container = "actors";

        public ActorController(MoviesApiDbContext context, IMapper mapper, IStorage storage)
        {
            this.context = context;
            this.mapper = mapper;
            this.storage = storage;
        }

        [HttpGet]
        public async Task<List<ActorDTO>> GetActors([FromQuery] PagingDTO pagingDTO)
        {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertParams(queryable, pagingDTO.ItemsPerPage);


            var entities = await queryable.Paginate(pagingDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(entities);
        }

        [HttpGet("{id}", Name = "GetOneActor")]
        public async Task<ActionResult<ActorDTO>> GetOneActor(int id)
        {
            var entity = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<ActorDTO>(entity);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> CreateActor([FromForm] ActorCreationDTO NewActor)
        {

            var entity = mapper.Map<Actor>(NewActor);

            if (NewActor.Picture != null)
            {
                using (var stream = new MemoryStream())
                {
                    await NewActor.Picture.CopyToAsync(stream);
                    var content = stream.ToArray();
                    var extension = Path.GetExtension(NewActor.Picture.FileName);
                    entity.Picture = await storage.StoreFile(content, extension, container, NewActor.Picture.ContentType);
                }
            }

            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entity);
            return new CreatedAtRouteResult("GetOneActor", new { id = entity.Id }, dto);
        }




        [HttpPut("{id}")]
        public async Task<ActionResult> EditActor(int id, [FromForm] ActorCreationDTO EditedActor)
        {

            var actorDB = await context.Actors.FirstOrDefaultAsync(a => a.Id == id);
            if (actorDB == null) { return NotFound(); }

            actorDB = mapper.Map(EditedActor, actorDB);

            if (EditedActor.Picture != null)
            {
                using (var stream = new MemoryStream())
                {
                    await EditedActor.Picture.CopyToAsync(stream);
                    var content = stream.ToArray();
                    var extension = Path.GetExtension(EditedActor.Picture.FileName);
                    actorDB.Picture = await storage.EditFile(content, extension, container, actorDB.Picture, EditedActor.Picture.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> EditField(int id, [FromBody] JsonPatchDocument<PatchActorDTO> patchDocument ) 
        {
            if( patchDocument == null)
            {
                return BadRequest();
            }

            var entityDB = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entityDB == null) {  return NotFound (); }
            
            var entityDTO = mapper.Map<PatchActorDTO>(entityDB);

            patchDocument.ApplyTo(entityDTO, ModelState);

            var isValid = TryValidateModel(entityDTO);

            if(!isValid) 
            
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entityDTO, entityDB);

            await context.SaveChangesAsync();

            return NoContent();


        }



        [HttpDelete("{id}")]
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
