using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class ActorCreationDTO
    {

        public IFormFile Picture { get; set; }
    }
}
