namespace Application.Services.Abstractions;

public interface INotifService : ICRUDService<Notif>
{
    Task<Notif> SaveNotifAsync(NotifVM entity,  CancellationToken ct);
    Task SaveNotifAsync(IEnumerable<NotifVM> entities, CancellationToken ct);
    Task ScheduleNotificationAsync(Notif entity, CancellationToken ct);
    Task SendNotificationAsync(IEnumerable<NotifRq> messages);
    Task<IEnumerable<Notif>> GetUnDeliveredAsync();
    Task MarkNotificationsAsReadAsync(List<Notif> notifs, CancellationToken tc);
    Task<bool> MarkNotificationsAsReadAsync(Notif notif, CancellationToken ct);
    Task MarkNotificationAsFailedAttemp(Notif notif, CancellationToken ct);
}
