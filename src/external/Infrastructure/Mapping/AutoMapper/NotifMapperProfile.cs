using AutoMapper;


namespace Infrastructure.Mapping.AutoMapper;

public class NotifMapperProfile : Profile
{
    public NotifMapperProfile()
    {
        CreateMap<Notif, CreateNotifRq>().ReverseMap();
    }
    
}
