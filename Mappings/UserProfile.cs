using AutoMapper;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, Models.DTOs.User.Response.UserDTO>();

            CreateMap<User, Models.DTOs.Admin.Response.UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}
