using AutoMapper;
using EzhikLoader.Admin.Entities;
using EzhikLoader.Admin.Models.Subscription;

namespace EzhikLoader.Admin.Mappings
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Subscription, SubscriptionResponseDTO>()
                .ForMember(dest => dest.UserLogin, opt => opt.MapFrom(src => src.User.Login))
                .ForMember(dest => dest.AppName, opt => opt.MapFrom(src => src.App.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.IsCanceled && src.EndDate > DateTime.UtcNow));

            CreateMap<CreateSubscriptionRequestDTO, Subscription>();
        }
    }
}
