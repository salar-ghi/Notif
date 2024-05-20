namespace Presentation.Configuration;

public class SendNotifBackgroundService
{
    private readonly INotifManagementService _notifManagement;
    public SendNotifBackgroundService(INotifManagementService notifManagement)
    {
        _notifManagement = notifManagement;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    //[AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 2 })]
    public async Task ExecuteAsync()
    {
        CancellationToken ct = default(CancellationToken);
        while (!ct.IsCancellationRequested)
        {
            Console.WriteLine($"Running Execute Method() 'SendNotif' at: {DateTime.Now}");
            var notif = await _notifManagement.SendNotif();
            //using (var scope  = _scopeFactory.CreateScope())
            //{
            //    var scopedService = scope.ServiceProvider.GetRequiredService<INotifManagementService>();
            //    Console.WriteLine($"Running method 'CheckCache' at: {DateTime.Now}");
            //    //var item = scopedService.CheckCacheAndSaveToStorage();
            //}

            // Your method logic here
            //await _saveNotifToStorageJob.CheckCacheAndSaveToStorage(stoppingToken);
            await Task.Delay(2000); 
        }
    }
}
