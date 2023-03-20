using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Movie : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool InCinema { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }

        public List<Movies_Actors> Movies_Actors { get; set; }
        public List<Movies_Genres> Movies_Genres { get; set; }
        public List<Movies_Rooms> Movies_Rooms { get; set; }
    }
}
