using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MoviesApi.DTOs;
using MoviesApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace MoviesApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile(GeometryFactory geometryFactory)
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();

            CreateMap<IdentityUser, UserDTO>();



            CreateMap<Room, RoomDTO>()
                .ForMember(x => x.Latitude, x => x.MapFrom(y => y.Location.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(y => y.Location.X));


            CreateMap<RoomCreationDTO, Room>()
                .ForMember(x=> x.Location, x=> x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Latitude, y.Longitude))));

            CreateMap<RoomCreationDTO, Room>()
                .ForMember(x => x.Location, x => x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Latitude, y.Longitude))));



            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>().ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<PatchActorDTO, Actor>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.Movies_Genres, options => options.MapFrom(MapMovies_Genres))
                .ForMember(x => x.Movies_Actors, options => options.MapFrom(MapMovies_Actors));

            CreateMap<Movie, MovieDetailDTO>()
                .ForMember( x => x.Genres, options => options.MapFrom(MapMovieGenres))
                .ForMember( x => x.Actors, options => options.MapFrom(MapMovieActors));


            CreateMap<PatchMovieDTO, Movie>().ReverseMap();

        }

        private List<ActorMovieDetailDTO> MapMovieActors(Movie movie, MovieDetailDTO movieDetailDTO)
        {
            var result = new List<ActorMovieDetailDTO>();

            if (movie.Movies_Actors == null) { return result; }
            foreach (var movieActor in movie.Movies_Actors)
            {
                result.Add(new ActorMovieDetailDTO() { ActorId = movieActor.ActorId, Name = movieActor.Actor.Name , Character = movieActor.Character });
            }

            return result;
        }


        private List<GenreDTO> MapMovieGenres(Movie movie, MovieDetailDTO movieDetailDTO)
        {
            var result = new List<GenreDTO>();
            if (movie.Movies_Genres == null) { return result; }
            foreach (var movieGenre in movie.Movies_Genres)
            {
                result.Add(new GenreDTO() { Id = movieGenre.GenreId, Name = movieGenre.Genre.Name });
            }
            return result;
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
