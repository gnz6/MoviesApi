using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : CustomBaseController
    {

        private readonly IMapper mapper;
        private readonly MoviesApiDbContext context;

        public RoomController(MoviesApiDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task <ActionResult<List<RoomDTO>>> Get()
        {
            return await Get<Room, RoomDTO>();
        }

        [HttpGet("{id}", Name ="getRoom")]
        public async Task<ActionResult<RoomDTO>>Get(int id)
        {
            return await Get<Room, RoomDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RoomCreationDTO roomCreationDTO)
        {
            return await Post<RoomCreationDTO, Room, RoomDTO>(roomCreationDTO, "getRoom");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] RoomCreationDTO roomCreationDTO)
        {
            return await Put<RoomCreationDTO, Room>(id, roomCreationDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>Delete(int id)
        {
            return await Delete<Room>(id);  
        }
    } 
}
