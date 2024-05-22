using Application.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Mapping.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //CreateMap<Notif, NotifRq>().ReverseMap();
        //CreateMap<Recipient, RecipientRq>().ReverseMap();

        CreateMap<Notif, NotifVM>()
            .ForMember(dest => dest.SendDate, op => op.MapFrom(src => src.NextTry))
            .ForMember(dest => dest.Provider, op => op.MapFrom(src => src.ProviderID))
            .ReverseMap();
            
        CreateMap<Recipient, RecipientVM>().ReverseMap();




        //CreateMap<NotifType, ProviderType>()
        //    .ConvertUsing(src => (ProviderType)src);
        //CreateMap<Notif, Provider>()
        //    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));


        CreateMap<NotifType, ProviderType>().ConvertUsing((value, destination) =>
        {
            switch (value)
            {
                case NotifType.SMS:
                    return ProviderType.Mobile;
                case NotifType.Email:
                    return ProviderType.Email;
                case NotifType.Signal:
                    return ProviderType.InApp;
                case NotifType.MessageBrocker:
                    return ProviderType.MessageBrocker;
                case NotifType.Telegram:
                    return ProviderType.SocialMedia;
                case NotifType.Whatsapp:
                    return ProviderType.SocialMedia;
                default:
                    return ProviderType.None;
            }
        });

    }
    
}
