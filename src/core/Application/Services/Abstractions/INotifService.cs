namespace Application.Services.Abstractions;

public interface INotifService : ICRUDService<Notif>
{
    Task<Notif> SaveNotifAsync(NotifVM entity,  CancellationToken ct);
    Task SaveNotifAsync(IEnumerable<NotifRq> entities, CancellationToken ct);
    Task ScheduleNotificationAsync(Notif entity, CancellationToken ct);

    Task SendNotificationAsync(IEnumerable<NotifRq> messages);

    //#region Cache
    //Task<bool> CacheNotifAsync(IEnumerable<NotifVM> entities, CancellationToken cancellationToken = default(CancellationToken));
    //Task<IEnumerable<NotifVM>> GetAllNotifAsync(CancellationToken cancellationToken = default(CancellationToken));

    //#endregion
}
