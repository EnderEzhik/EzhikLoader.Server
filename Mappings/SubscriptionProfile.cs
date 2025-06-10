using AutoMapper;
using EzhikLoader.Server.Models;

namespace EzhikLoader.Server.Mappings
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Subscription, Models.DTOs.Admin.Response.SubscriptionDTO>();
        }
    }
}
