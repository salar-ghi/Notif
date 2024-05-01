namespace Application.Services.Abstractions;

public interface INotifService
{
    Task<Notif> SaveNotifAsync(CreateNotifRq entity,  CancellationToken ct);
    Task SaveNotifAsync(IEnumerable<CreateNotifRq> entities, CancellationToken ct);

    Task ScheduleNotificationAsync(Notif entity, CancellationToken ct);


    Task<Notif> UpdateNotifAsync(Notif entity);
    Task UpdateNotifAsync(IEnumerable<Notif> entities);
}
