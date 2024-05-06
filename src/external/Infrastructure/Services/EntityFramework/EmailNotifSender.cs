namespace Infrastructure.Services.EntityFramework;

public class EmailNotifSender : IEmailProvider, INotifSender
{
    public Task SendNotificationAsync(string message)
    {
        Console.WriteLine($"Sending Email notification: {message}");
        return Task.CompletedTask;
    }
}
