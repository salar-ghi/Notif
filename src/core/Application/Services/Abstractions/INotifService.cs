using Application.Models.Responses;

namespace Application.Services.Abstractions;

public interface INotifService : ICRUDService<Notif>
{
    Task<Notif> SaveNotifAsync(NotifRq entity,  CancellationToken ct);
    Task SaveNotifAsync(IEnumerable<NotifRq> entities, CancellationToken ct);
    Task ScheduleNotificationAsync(Notif entity, CancellationToken ct);

    Task SendNotificationAsync(IEnumerable<NotifRq> messages);
    Task<bool> CacheNotifAsync(IEnumerable<NotifRq> entities, CancellationToken cancellationToken = default(CancellationToken));
    Task<IEnumerable<NotifRs>> GetAllNotifAsync(CancellationToken cancellationToken = default(CancellationToken));

}
