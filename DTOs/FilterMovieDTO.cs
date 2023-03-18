namespace MoviesApi.DTOs
{
    public class FilterMovieDTO
    {
        public int Page { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 10;

        public PagingDTO Paging
        {
            get 
            {
                return new PagingDTO { Page = Page,ItemsPerPage = ItemsPerPage };
            }
        }
        public string Title { get; set; }

        public int GenreId { get; set; }

        public bool InCinema { get; set; }

        public bool NextRelease { get; set; }

        public string OrderField { get; set; }

        public bool AscOrder { get; set; } = true;


    }
}
