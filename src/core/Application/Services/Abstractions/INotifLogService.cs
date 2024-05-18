namespace Application.Services.Abstractions;

public interface INotifLogService : ICRUDService<NotifLog>
{
    Task<NotifLog> SaveNotifLogAsync(NotifLog entity, CancellationToken ct);

    Task<bool> SaveNotifLogAsync(ICollection<NotifLog> entity, CancellationToken ct);

    Task<NotifLog> GetNotifLog(long Id);

    Task MarkNotifLogAsFailed(NotifLog log, CancellationToken ct);
    Task MarkNotifLogAsSuccess(NotifLog log, CancellationToken ct);
}
