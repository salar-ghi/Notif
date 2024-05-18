using Application.Models.Responses;

namespace Application.Services.Abstractions;

public interface INotifSender
{
    Task<bool> SendNotifAsync(string providerName, Notif notif);

}
