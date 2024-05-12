using Application.Models.Responses;

namespace Presentation.Controllers;


[EnableCors(Constants.CorsPolicyName)]
//[EnableCors("MyPolicy")]
//[Route("api/v{version:apiVersion}/[controller]")]
public class NotifController : BaseController<NotifController, ApplicationSettingExtenderModel>
{
    private readonly INotifService _notifService;
    //private readonly IMemoryCache _memoryCache;
    //private readonly string MessageCollectionKey = "messagesCollectionKey";

    public NotifController(INotifService notifService)
    {
        _notifService = notifService;
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Index()
    {
        string item = "hello every body and how are you ";
        return Ok(item);
    }

    [HttpPost("SendNotif")]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
    //[ProducesResponseType(typeof(OkListResult<>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> SendNotifAsync([FromBody] IEnumerable<NotifRq> notifRq, CancellationToken cancellationToken = default)
    {
        var result = await _notifService.CacheNotifAsync(notifRq, cancellationToken);
        return Ok(result);

        //await _notifService.ScheduleNotificationAsync(notif, cancellationToken);

        //await _notifService.SendNotificationAsync(notifRq);
    }

    [HttpGet("AllCaches")]
    [ProducesResponseType(typeof(OkListResult<NotifRs>),StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAllCacheNotifs(CancellationToken cancellation = default(CancellationToken))
    {
        var items = await _notifService.GetAllNotifAsync(cancellation);
        return Ok(items);
    }

    [HttpGet("Send")]
    [MapToApiVersion("1.0")]    
    public async Task<IActionResult> SendAsync()
    {
        string item = "hello every body and how are you ";
        //return Ok(JsonContent(item));
        return new JsonResult(item);
    }


}
