using AutoMapper;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Models.DTOs.Admin.Request;

namespace EzhikLoader.Server.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDTO, User>();

            CreateMap<User, Models.DTOs.User.Response.UserDTO>();

            CreateMap<User, Models.DTOs.Admin.Response.UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}
