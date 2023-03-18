using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public List<Movies_Actors> Movies_Actors { get; set; }
        public List<Movies_Genres> Movies_Genres { get; set; }
    }
}
