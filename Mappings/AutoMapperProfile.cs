using AutoMapper;
using EzhikLoader.Server.Models;
using EzhikLoader.Server.Models.DTOs.Admin.Request;
using EzhikLoader.Server.Models.DTOs.Admin.Response;
using EzhikLoader.Server.Models.DTOs.User.Response;

namespace EzhikLoader.Server.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<App, AppDTO>();

            CreateMap<App, UpdateAppDataDTO>();

            CreateMap<Subscription, Models.DTOs.Admin.Response.SubscriptionDTO>();
        }
    }
}
