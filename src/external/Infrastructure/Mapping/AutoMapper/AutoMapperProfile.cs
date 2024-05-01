namespace Infrastructure.Mapping.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Notif, CreateNotifRq>().ReverseMap();
        CreateMap<Recipient, RecipientRq>().ReverseMap();

    }
    
}
