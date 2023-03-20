using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Room : IId
    {

        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public List <Movies_Rooms> MoviesRooms { get; set; }

        public Point Location { get; set; }

    }
}
