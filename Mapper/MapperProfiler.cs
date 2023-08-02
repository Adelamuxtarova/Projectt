using AutoMapper;
using Project.Models.DTO;
using Project.Entities;

namespace Project.Mapper
{
    public class MapperProfiler : Profile
    {
        public MapperProfiler()
        {
         CreateMap<Users,AddUserDTO>().ReverseMap();
        }
    }
}
