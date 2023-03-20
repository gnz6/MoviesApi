using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}
