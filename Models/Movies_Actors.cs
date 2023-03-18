namespace MoviesApi.Models
{
    public class Movies_Actors
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }

        public string Character { get; set; }
        public int Order { get; set; }
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }

    }
}
