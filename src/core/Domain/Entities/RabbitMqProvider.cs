namespace Domain.Entities;

public class RabbitMqProvider : Provider
{
    public override Task<bool> SendNotifAsync(Notif notif)
    {
        // Logic to send notification via RabbitMq
        throw new NotImplementedException();
    }
}
