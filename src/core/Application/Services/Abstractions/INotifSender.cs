namespace Application.Services.Abstractions;

public interface INotifSender
{
    Task SendNotificationAsync(Notif notif, string providerName);

}
