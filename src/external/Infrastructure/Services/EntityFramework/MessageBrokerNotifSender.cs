
namespace Infrastructure.Services.EntityFramework;

public class MessageBrokerNotifSender : IMessageBrockerProvider, INotifSender
{
    public Task SendNotificationAsync(Notif notif, string providerName)
    {
        Console.WriteLine($"Sending Message Brocker notification: {notif.Message}");
        return Task.CompletedTask;
    }
}
