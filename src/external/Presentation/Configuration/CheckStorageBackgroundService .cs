namespace Presentation.Configuration;

public class CheckStorageBackgroundService //: BackgroundService
{
    private readonly IMessageManagementService _messageManagement;
    public CheckStorageBackgroundService(IMessageManagementService messageManagement)
    {
        _messageManagement = messageManagement;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    //[AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 2 })]
    public async Task ExecuteAsync()
    {
        CancellationToken ct = default(CancellationToken);
        while (!ct.IsCancellationRequested)
        {
            Console.WriteLine($"Running Execute Method() 'CheckCache' at: {DateTime.Now}");
            await _messageManagement.SaveMessagesToStorage();

            Console.WriteLine($"Running Execute Method() 'SendNotif' at: {DateTime.Now}");
            var notif = await _messageManagement.SendMessages();

            await Task.Delay(3000);
        }
    }
}
