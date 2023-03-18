using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

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



    }
}
