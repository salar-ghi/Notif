namespace Infrastructure.Services.EntityFramework;

public class MessageBrokerNotifSender : IMessageBrockerProvider, INotifSender
{
    public Task SendNotificationAsync(string message)
    {
        throw new NotImplementedException();
    }
}
