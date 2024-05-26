namespace Presentation.Configuration;

public class SendMessageBackgroundService
{
    private readonly IMessageManagementService _messageManagement;
    public SendMessageBackgroundService(IMessageManagementService messageManagement)
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
            Console.WriteLine($"Running Execute Method() 'SendNotif' at: {DateTime.Now}");
            var notif = await _messageManagement.SendMessages();
            //using (var scope  = _scopeFactory.CreateScope())
            //{
            //    var scopedService = scope.ServiceProvider.GetRequiredService<INotifManagementService>();
            //    Console.WriteLine($"Running method 'CheckCache' at: {DateTime.Now}");
            //    //var item = scopedService.CheckCacheAndSaveToStorage();
            //}
            await Task.Delay(2000); 
        }
    }
}
