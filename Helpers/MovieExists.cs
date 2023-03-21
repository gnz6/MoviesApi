//using Microsoft.AspNetCore.Mvc.Filters;
//using MoviesApi.Models;

//namespace MoviesApi.Helpers
//{
//    public class MovieExists : Attribute, IAsyncResourceFilter
//    {
//        private readonly MoviesApiDbContext dbcontext;

//        public MovieExists(MoviesApiDbContext dbcontext)
//        {
//            this.dbcontext = dbcontext;
//        }

//        public Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
//        {
//            var movieObject = context.HttpContext.Request.RouteValues["movieId"];

//            if(movieObject == null)
//            {
//                return;
//            }

//            var movieId = int.Parse(movieObject.ToString());


//        }


//    }
//}
