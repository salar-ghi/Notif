namespace Application.Services.Abstractions;

public interface IMessageService : ICRUDService<Message>
{
    Task<Message> SaveMessageAsync(MessageVM entity,  CancellationToken ct);
    Task SaveMessageAsync(IEnumerable<MessageVM> entities, CancellationToken ct);
    Task ScheduleMessageAsync(Message entity, CancellationToken ct);
    Task SendMessageAsync(IEnumerable<MessageRq> messages);
    Task<IEnumerable<Message>> GetUnDeliveredAsync();
    Task MarkMessagesAsReadAsync(List<Message> notifs, CancellationToken tc);
    Task<bool> MarkMessagesAsReadAsync(Message notif, CancellationToken ct);
    Task MarkMessageAsFailedAttemp(Message notif, CancellationToken ct);
}
