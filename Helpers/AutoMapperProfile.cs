﻿using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.Models;

namespace MoviesApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap< GenreCreationDTO, Genre>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>().ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<PatchActorDTO, Actor>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreationDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore());

            CreateMap<PatchMovieDTO, Movie>().ReverseMap();

        }
    }
}
