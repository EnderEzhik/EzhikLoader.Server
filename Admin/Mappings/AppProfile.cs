using AutoMapper;
using EzhikLoader.Admin.Entities;
using EzhikLoader.Admin.Models.App;

namespace EzhikLoader.Admin.Mappings
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<App, AppResponseDTO>();
            CreateMap<CreateAppRequestDTO, App>();
        }
    }
}
