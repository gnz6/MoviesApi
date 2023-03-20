using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class RoomCreationDTO
    {

        [Required]
        [StringLength(120)]
        public string Name { get; set; }
    }
}
