using Application.Models.Responses;

namespace Application.Services.Abstractions;

public interface INotifSender
{
    //Task SendNotificationAsync(Notif notif, string providerName);
    Task<ProviderRs> SendNotificationAsync(Notif notif, string providerName);
}
