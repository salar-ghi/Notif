namespace Application.Services.Abstractions;

public interface INotifService
{
    Task<Notif> CreateNotifAsync(Notif entity);
    Task CreateNotifAsync(ICollection<Notif> entities);

    Task<Notif> UpdateNotifAsync(Notif entity);
    Task UpdateNotifAsync(ICollection<Notif> entities);


}
