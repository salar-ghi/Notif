namespace Application.Services.Abstractions;

public interface INotifSender
{
    Task SendNotificationAsync(string message);

}
