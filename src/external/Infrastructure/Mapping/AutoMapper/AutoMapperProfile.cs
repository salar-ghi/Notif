namespace Infrastructure.Mapping.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Notif, NotifRq>().ReverseMap();
        CreateMap<Recipient, RecipientRq>().ReverseMap();

    }
    
}
