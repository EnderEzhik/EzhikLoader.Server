using AutoMapper;
using EzhikLoader.Admin.Entities;
using EzhikLoader.Admin.Models.User;

namespace EzhikLoader.Admin.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDTO>();
            CreateMap<CreateUserRequestDTO, User>();
        }
    }
}
