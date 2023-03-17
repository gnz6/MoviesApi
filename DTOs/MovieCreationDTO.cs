using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class MovieCreationDTO : PatchMovieDTO
    {
        
        [FileSizeValidation(4)]
        [FileTypeValidation(GroupTypeFile.Image)]
        public IFormFile Poster { get; set; }
    }
}
