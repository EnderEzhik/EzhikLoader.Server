using AutoMapper;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Mappings
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<Models.DTOs.Admin.Request.CreateAppDTO, App>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.FileName))
                .ForMember(dest => dest.IconName, opt => opt.MapFrom(src => src.Icon.FileName));

            CreateMap<Models.DTOs.Admin.Request.UpdateAppDTO, App>();

            CreateMap<App, Models.DTOs.User.Response.AppDTO>();
        }
    }
}
