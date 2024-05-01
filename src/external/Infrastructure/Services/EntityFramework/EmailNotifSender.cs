namespace Infrastructure.Services.EntityFramework;

public class EmailNotifSender : IEmailProvider, INotifSender
{
    public Task SendNotificationAsync(string message)
    {
        throw new NotImplementedException();
    }
}
