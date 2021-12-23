using AutoMapper;
using ProjetoCore.API.Models;

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
