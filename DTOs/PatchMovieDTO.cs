using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class PatchMovieDTO
    {


        public int Id { get; set; }
        public string Title { get; set; }
        public bool InCinema { get; set; }
        public DateTime ReleaseDate { get; set; }


    }
}
