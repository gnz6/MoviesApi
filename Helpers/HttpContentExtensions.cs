using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Helpers
{
    public static class HttpContentExtensions
    {
            public async static Task InsertParams<T>(this HttpContext httpContext, IQueryable<T> queryable, int itemsPerPage)
        {
            double allItems = await queryable.CountAsync();
            double numberOfPages = Math.Ceiling(allItems / itemsPerPage);

            httpContext.Response.Headers.Add("pages", numberOfPages.ToString());
        }
    }
}
