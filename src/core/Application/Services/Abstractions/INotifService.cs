namespace Application.Services.Abstractions;

public interface INotifService : ICRUDService<Notif>
{
    Task<Notif> SaveNotifAsync(NotifVM entity,  CancellationToken ct);
    Task SaveNotifAsync(IEnumerable<NotifRq> entities, CancellationToken ct);
    Task ScheduleNotificationAsync(Notif entity, CancellationToken ct);

    Task SendNotificationAsync(IEnumerable<NotifRq> messages);
    Task<IEnumerable<Notif>> GetUnDeliveredAsync();

    Task MarkNotificationsAsReadAsync(List<Notif> notifs, CancellationToken tc);


}
