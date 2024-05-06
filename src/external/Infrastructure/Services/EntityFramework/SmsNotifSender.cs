namespace Infrastructure.Services.EntityFramework;

public class SmsNotifSender : ISmsProvider, INotifSender
{
    
    
    
    
    public Task SendNotificationAsync(string message)
    {
        Console.WriteLine($"Sending SMS notification: {message}");
        return Task.CompletedTask;
    }


}
