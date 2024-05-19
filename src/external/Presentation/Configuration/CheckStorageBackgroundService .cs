namespace Presentation.Configuration;

public class CheckStorageBackgroundService : BackgroundService
{
    //private readonly IServiceScopeFactory _scopeFactory;
    //private readonly INotifManagementService _saveNotifToStorageJob;

    public CheckStorageBackgroundService()
    {
        //_scopeFactory = scopeFactory;
        //_saveNotifToStorageJob = saveNotifToStorageJob;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Running method 'CheckCache' at: {DateTime.Now}");

            //using (var scope  = _scopeFactory.CreateScope())
            //{
            //    var scopedService = scope.ServiceProvider.GetRequiredService<INotifManagementService>();
            //    Console.WriteLine($"Running method 'CheckCache' at: {DateTime.Now}");
            //    //var item = scopedService.CheckCacheAndSaveToStorage();
            //}

            // Your method logic here
            //await _saveNotifToStorageJob.CheckCacheAndSaveToStorage(stoppingToken);            

            await Task.Delay(2000); // 2 seconds delay
        }
    }
}
