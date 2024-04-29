namespace Application.Services.Abstractions;

public interface INotifService
{
    Task<Notif> CreateNotifAsync(CreateNotifRq entity);
    Task CreateNotifAsync(ICollection<CreateNotifRq> entities);

    Task<Notif> UpdateNotifAsync(Notif entity);
    Task UpdateNotifAsync(ICollection<Notif> entities);
}
