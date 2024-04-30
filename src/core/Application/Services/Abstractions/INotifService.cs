namespace Application.Services.Abstractions;

public interface INotifService
{
    Task<Notif> CreateNotifAsync(CreateNotifRq entity,  CancellationToken ct);
    Task CreateNotifAsync(IEnumerable<CreateNotifRq> entities, CancellationToken ct);

    Task<Notif> UpdateNotifAsync(Notif entity);
    Task UpdateNotifAsync(IEnumerable<Notif> entities);
}
