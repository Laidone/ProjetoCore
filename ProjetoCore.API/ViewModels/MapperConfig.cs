using AutoMapper;
using Dominio;

namespace ProjetoCore.API.ViewModels
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Movie, MovieViewModel>().ReverseMap();
        }
    }
}
