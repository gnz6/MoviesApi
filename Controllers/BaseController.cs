using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Helpers;
using MoviesApi.Models;
using System.Xml.XPath;

namespace MoviesApi.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly MoviesApiDbContext context;
        private readonly IMapper mapper;

        public CustomBaseController(MoviesApiDbContext context , IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;

        }

        protected async Task <List<TDTO>> Get <TEntity, TDTO>() where TEntity : class
        {
            var entities = await context.Set<TEntity>().AsNoTracking().ToListAsync();
            var dtos = mapper.Map<List<TDTO>>(entities);
            return dtos;
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity, TDTO>(int id) where TEntity : class, IId
        {
            var entity = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync( x => x.Id == id);
            if ( entity == null) { return NotFound(); }
            return mapper.Map<TDTO>(entity);
        }


        protected async Task<ActionResult> Post<TCreation, TEntity, TRead>(TCreation creationDTO, string routeName) where TEntity : class, IId
        {
            var entity = mapper.Map<TEntity>(creationDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            var dtoRead = mapper.Map<TRead>(entity);

            return new CreatedAtRouteResult(routeName, new { id = entity.Id }, dtoRead);
        }


        protected async Task<ActionResult> Put<TCreation, TEntity>( int id, TCreation creationDTO) where TEntity : class, IId
        {
            var entity = mapper.Map<TEntity>(creationDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntity>(int id) where TEntity : class, IId, new()
        {
            var exist = await context.Set<TEntity>().AnyAsync(g => g.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new TEntity() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

        protected async Task<List<TDTO>> Get<TEntity,TDTO>(PagingDTO pagingDTO) where TEntity : class, IId
        {
            var queryable = context.Set<TEntity>().AsQueryable();
            await HttpContext.InsertParams(queryable, pagingDTO.ItemsPerPage);


            var entities = await queryable.Paginate(pagingDTO).ToListAsync();
            return mapper.Map<List<TDTO>>(entities);
        }


        protected async Task <ActionResult> Patch<TEntity, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument) where TDTO: class where TEntity : class, IId
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entityDB = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (entityDB == null) { return NotFound(); }

            var entityDTO = mapper.Map<TDTO>(entityDB);

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

    }
}
