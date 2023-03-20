namespace MoviesApi.Models
{
    public class Movies_Rooms
    {
        public int MovieId { get; set; }
        public int RoomId { get; set;}
        public Movie Movie { get; set; }
        public Room Room { get; set; }

    }
}
