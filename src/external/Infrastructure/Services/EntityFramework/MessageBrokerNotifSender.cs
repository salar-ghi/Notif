namespace Infrastructure.Services.EntityFramework;

public class MessageBrokerNotifSender : IMessageBrockerProvider, INotifSender
{
    public Task SendNotificationAsync(string message)
    {
        Console.WriteLine($"Sending Message Brocker notification: {message}");
        return Task.CompletedTask;
    }
}
