namespace Presentation.Configuration;

public class CheckStorageBackgroundService : BackgroundService
{
    //private readonly ISaveNotifToStorageJob _saveNotifToStorageJob;
    public CheckStorageBackgroundService()
    {
            
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Your method logic here
            Console.WriteLine($"Running your method at: {DateTime.Now}");
            //await _saveNotifToStorageJob.Run();

            await Task.Delay(2000); // 2 seconds delay
        }
    }
}
