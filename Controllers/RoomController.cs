using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Models;
using NetTopologySuite.Geometries;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : CustomBaseController
    {

        private readonly IMapper mapper;
        private readonly GeometryFactory geometryFactory;
        private readonly MoviesApiDbContext context;

        public RoomController(MoviesApiDbContext context, IMapper mapper, GeometryFactory geometryFactory) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.geometryFactory = geometryFactory;
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


        [HttpGet("near")]
        public async Task<ActionResult<List<RoomNearDTO>>> Near([FromQuery] RoomNearFilterDTO filter)
        {
            var userLocation = geometryFactory.CreatePoint(new Coordinate(filter.Longitude, filter.Latitude));

            var rooms = await context.Rooms.OrderBy(x => x.Location.Distance(userLocation)).Where(x => x.Location.IsWithinDistance(userLocation, filter.KmDistance * 1000)).Where(x => x.Location.IsWithinDistance(userLocation, filter.KmDistance * 1000))
                .Select(x => new RoomNearDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitude = x.Location.Y,
                    Longitude = x.Location.X,
                    DistanceMts = Math.Round(x.Location.Distance(userLocation))
                })
                .ToListAsync();
            
            return rooms;
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
