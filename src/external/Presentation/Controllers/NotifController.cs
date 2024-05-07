using Application.Models;
using Application.Models.Requests;
using Hangfire;
using Microsoft.Extensions.Caching.Memory;
using WebCore.Controllers;

namespace Presentation.Controllers;

[ApiController]
//[ApiExplorerSettings(IgnoreApi = false)]
//[Route("api/v{version:apiVersion}/[controller]")]
//[EnableCors(Constants.CorsPolicyName)]
public class NotifController : ControllerBase
    //BaseController<NotifController, ApplicationSettingExtenderModel>
{
    private readonly INotifService _notifService;
    private readonly IMemoryCache _memoryCache;
    private readonly string MessageCollectionKey = "messagesCollectionKey";

    public NotifController(INotifService notifService, IMemoryCache memoryCache)
    {
        _notifService = notifService;
        _memoryCache = memoryCache;

    }

    [HttpGet("Index")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Index()
    {
        //string item = "hello every body and how are you ";
        //return Ok(JsonContent(item));
        return Ok();
    }

    [HttpPost("SendNotif")]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(OkListResult<>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> SendNotifAsync([FromBody] IEnumerable<CreateNotifRq> notifRq, CancellationToken cancellationToken = default)
    {

        //var notif =
        await _notifService.SaveNotifAsync(notifRq, cancellationToken);

        //await _notifService.ScheduleNotificationAsync(notif, cancellationToken);

        await _notifService.SendNotificationAsync(notifRq);


        return Ok("hello everyone");
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
