namespace Application.Services.Abstractions;

public interface INotifManagementService
{
    Task<bool> CheckCacheAndSaveToStorage(CancellationToken ct = default(CancellationToken));
    Task<bool> SendNotif(CancellationToken ct = default(CancellationToken));
}
