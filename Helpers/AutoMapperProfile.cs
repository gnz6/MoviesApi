using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.Models;

namespace MoviesApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
        }
    }
}
