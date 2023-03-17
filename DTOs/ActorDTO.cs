using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Picture { get; set; }
    }
}
