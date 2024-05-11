
namespace Infrastructure.Services.EntityFramework;

public class EmailNotifSender : IEmailProvider //, INotifSender
{
    public Task SendNotificationAsync(string message)
    {
        Console.WriteLine($"Sending Email notification: {message}");
        return Task.CompletedTask;
    }

    public Task SendNotificationAsync(Notif notif, string providerName)
    {
        Console.WriteLine($"Sending Email notification: {notif.Message}");
        return Task.CompletedTask;
    }
}
