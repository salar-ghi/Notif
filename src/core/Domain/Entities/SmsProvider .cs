namespace Domain.Entities;

public class SmsProvider : Provider
{
    public override async Task<bool> SendNotifAsync(Notif notif)
    {
        return await Task.FromResult(false);
        // Logic to send notification via SMS
    }
}
