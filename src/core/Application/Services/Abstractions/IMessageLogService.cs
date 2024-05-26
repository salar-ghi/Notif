namespace Application.Services.Abstractions;

public interface IMessageLogService : ICRUDService<MessageLog>
{
    Task<MessageLog> SaveMessageLogAsync(MessageLog entity, CancellationToken ct);

    Task<bool> SaveMessageLogAsync(ICollection<MessageLog> entity, CancellationToken ct);

    Task<MessageLog> GetMessageLog(long Id);

    Task MarkMessageLogAsFailed(MessageLog log, CancellationToken ct);
    Task MarkMessageLogAsSuccess(MessageLog log, CancellationToken ct);
}
