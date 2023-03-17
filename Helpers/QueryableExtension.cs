using MoviesApi.DTOs;

namespace MoviesApi.Helpers
{
    public static class QueryableExtension
    {
 
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable , PagingDTO pagingDTO)
        {
            return queryable
                .Skip((pagingDTO.Page -1) * pagingDTO.ItemsPerPage )
                .Take(pagingDTO.ItemsPerPage);
        }

    }
}
