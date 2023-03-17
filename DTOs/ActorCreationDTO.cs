using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class ActorCreationDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }
}
