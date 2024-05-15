using Application.Models.Responses;

namespace Application.Services.Abstractions;

public interface INotifSender
{

    Task SendNotif(Notif notif);
    Task SendNotifAsync(Notif notif);

}
