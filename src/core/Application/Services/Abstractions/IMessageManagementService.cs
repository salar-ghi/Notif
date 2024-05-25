namespace Application.Services.Abstractions;

public interface IMessageManagementService
{
    Task<bool> SaveMessagesToStorage(CancellationToken ct = default(CancellationToken));
    Task<bool> SendMessages(CancellationToken ct = default(CancellationToken));
}
