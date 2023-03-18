namespace MoviesApi.Models
{
    public class Movies_Genres
    {
        public int GenreId { get; set; }
        public int MovieId { get; set; }
        public Genre Genre { get; set; }
        public Movie Movie { get; set; }

    }
}
