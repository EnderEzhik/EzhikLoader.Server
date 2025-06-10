using AutoMapper;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Mappings
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<App, Models.DTOs.User.Response.AppDTO>();
        }
    }
}
