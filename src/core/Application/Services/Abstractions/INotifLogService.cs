namespace Application.Services.Abstractions;

public interface INotifLogService : ICRUDService<NotifLog>
{
    Task<NotifLog> SaveNotifLogAsync(NotifLog entity, CancellationToken ct);
}
