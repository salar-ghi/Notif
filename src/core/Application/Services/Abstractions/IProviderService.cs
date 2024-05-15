namespace Application.Services.Abstractions;

public interface IProviderService : ICRUDService<Provider>
{
    Task<Provider> GetProvider(string name);
    Task<Provider> GetSpecificProvider(int Id);
    Task<Provider> GetSpecificProvider(string name);
    Task<Provider> GetRandomProvider(string? name, NotifType? type);



}
