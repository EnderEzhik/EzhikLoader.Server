using AutoMapper;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Models.DTOs.Response;

namespace EzhikLoader.Server.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<App, Models.DTOs.Response.AppDTO>();
        }
    }
}
