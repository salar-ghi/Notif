
namespace Domain.Entities;

public class EmailProvider : Provider
{
    public override Task<bool> SendNotifAsync(Notif notif)
    {
        // Logic to send notification via Email
        throw new NotImplementedException();
    }
}
