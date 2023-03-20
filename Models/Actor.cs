using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Actor : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Picture { get; set; }

        public List<Movies_Actors> Movies_Actors { get; set; }
        public List<Movies_Genres> Movies_Genres { get; set; }
    }
}
