using Application.Models;

namespace Infrastructure.Mapping.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Notif, NotifRq>().ReverseMap();
        CreateMap<Recipient, RecipientRq>().ReverseMap();

        CreateMap<Notif, NotifVM>()
            .ForMember(dest => dest.SendDate, op => op.MapFrom(src => src.NextTry))
            .ReverseMap();
            ;
        CreateMap<Recipient, RecipientVM>();

    }
    
}
