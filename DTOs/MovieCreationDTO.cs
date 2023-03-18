using Microsoft.AspNetCore.Mvc;
using MoviesApi.Helpers;
using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class MovieCreationDTO : PatchMovieDTO
    {
        
        [FileSizeValidation(4)]
        [FileTypeValidation(GroupTypeFile.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenreIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorMovieCreationDTO>>))]

        public List <ActorMovieCreationDTO> Actors { get; set; }
    }
}
