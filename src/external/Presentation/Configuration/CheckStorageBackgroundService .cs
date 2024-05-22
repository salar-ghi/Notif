using Presentation.Controllers;

namespace Presentation.Configuration;

public class CheckStorageBackgroundService //: BackgroundService
{

    private readonly INotifManagementService _notifManagement;
    public CheckStorageBackgroundService(INotifManagementService notifManagement)
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
            Console.WriteLine($"Running Execute Method() 'CheckCache' at: {DateTime.Now}");
            await _notifManagement.CheckCacheAndSaveToStorage();
            Console.WriteLine($"Running Execute Method() 'SendNotif' at: {DateTime.Now}");
            var notif = await _notifManagement.SendNotif();

            await Task.Delay(2000);
        }
    }
}
