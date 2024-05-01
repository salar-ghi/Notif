using Application.Models;
using Application.Models.Requests;
using Hangfire;
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
    public NotifController(INotifService notifService)
    {
        _notifService = notifService;
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
    public async Task<IActionResult> SendNotifAsync([FromBody] CreateNotifRq notifRq, CancellationToken cancellationToken = default)
    {
        var notif = await _notifService.SaveNotifAsync(notifRq, cancellationToken);

        await _notifService.ScheduleNotificationAsync(notif, cancellationToken);

        return Ok("hello everyone");
    }


    [HttpGet("Send")]
    //[ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
    //[ProducesResponseType(typeof(OkListResult<CancelledFundRs>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> SendAsync()
    {
        string item = "hello every body and how are you ";
        //return Ok(JsonContent(item));
        return new JsonResult(item);
    }


}
