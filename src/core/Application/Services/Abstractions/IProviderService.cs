namespace Application.Services.Abstractions;

public interface IProviderService : ICRUDService<Provider>
{
    Task<Provider> GetSpecificProvider(int Id, NotifType? type);
    Task<Provider> GetSpecificProvider(string name, NotifType? type);
    Task<Provider> GetRandomProvider(NotifType? type);



}
