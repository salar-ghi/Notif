using Application.Services.Abstractions;

namespace Presentation.Controllers;

[ApiController]
//[ApiExplorerSettings(IgnoreApi = false)]
//[Route("api/v{version:apiVersion}/[controller]")]
//[EnableCors(Constants.CorsPolicyName)]
public class NotifController : ControllerBase
{
    private readonly INotifService _notifService;
    public NotifController(INotifService notifService)
    {
        _notifService = notifService;
    }

    [HttpGet("")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Index()
    {
        //string item = "hello every body and how are you ";
        //return Ok(JsonContent(item));
        return Ok();
    }

    [HttpGet("")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(OkListResult<>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendNotifAsync([FromBody] NotifDto notifDto, CancellationToken cancellationToken)
    {
        var notifs = await _notifService.CreateNotifAsync(notifDto);

        return Ok("hello everyone");
    }


    [HttpGet("")]
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
