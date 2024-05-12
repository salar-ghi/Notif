using WebCore.Configuration;

namespace Presentation.Configuration;

public class ApplicationHealthCheck : HealthCheck
{
    //private readonly IBankService _bankService;
    public ApplicationHealthCheck(ILogger<ApplicationHealthCheck> logger) : base(logger)
    {
        //_bankService = bookService ?? throw new ArgumentNullException($"{nameof(IBankService)} is null");
    }

    public override async Task CheckHealthAsync()
    {
        //await _bankService.GetQuery().FirstOrDefaultAsync(e => e.Id == 1, default);

        throw new IndexOutOfRangeException("There is problem in application");

    }
}
