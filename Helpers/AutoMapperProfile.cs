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
            CreateMap<GenreCreationDTO, Genre>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>().ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<PatchActorDTO, Actor>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.Movies_Genres, options => options.MapFrom(MapMovies_Genres))
                .ForMember(x => x.Movies_Actors, options => options.MapFrom(MapMovies_Actors));



            CreateMap<PatchMovieDTO, Movie>().ReverseMap();

        }
        private List<Movies_Genres> MapMovies_Genres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<Movies_Genres>();
            if (movieCreationDTO == null)
            {
                return result;
            }

            foreach (var id in movieCreationDTO.GenreIds)
            {
                result.Add(new Movies_Genres { GenreId = id });
            }
            return result;
        }

        private List<Movies_Actors> MapMovies_Actors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<Movies_Actors>();
            if (movieCreationDTO.Actors == null)
            {
                return result;
            }

            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new Movies_Actors { ActorId = actor.ActorId, Character = actor.CharacterName });
            }
            return result;
        }

    }
}
