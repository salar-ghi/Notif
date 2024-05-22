namespace Presentation.Controllers;


[EnableCors(Constants.CorsPolicyName)]
public class NotifController : BaseController<NotifController, ApplicationSettingExtenderModel>
{
    private readonly INotifService _notifService;
    private readonly ICacheMessage _cache;
    private readonly INotifManagementService _notifManagementService;

    public NotifController(INotifService notifService, ICacheMessage cache, INotifManagementService notifManagementService)
    {
        _notifService = notifService;
        _cache = cache;
        _notifManagementService = notifManagementService;
    }

    [HttpGet("Index")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Index()
    {
        await _notifManagementService.CheckCacheAndSaveToStorage();
        return Ok();
    }

    [HttpPost("SendNotif")]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(OkListResult<NotifVM>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> SendNotifAsync([FromBody] IEnumerable<NotifVM> entities, CancellationToken cancellationToken = default)
    {
        var result = await _cache.AddMessage(entities);
        return Ok(result);
    }

    [HttpGet("AllCaches")]
    [ProducesResponseType(typeof(OkListResult<NotifRs>),StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAllCacheNotifs(CancellationToken cancellation = default(CancellationToken))
    {
        var notif = await _cache.GetAllMessages();
        return Ok(notif.ToList());
    }

    [HttpGet("")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> RunappAllCacheNotifs(CancellationToken cancellation = default(CancellationToken))
    {
        //await _notifManagementService.CheckCacheAndSaveToStorage();
        return Ok();
    }


    [HttpGet("Check")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> CheckNotifs(CancellationToken ct = default(CancellationToken))
    {
        try
        {
            await _notifManagementService.SendNotif();
            return Ok();
        }
        catch (Exception ex )
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

}
