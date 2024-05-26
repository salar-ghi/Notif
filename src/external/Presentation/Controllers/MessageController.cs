namespace Presentation.Controllers;


[EnableCors(Constants.CorsPolicyName)]
public class MessageController : BaseController<MessageController, ApplicationSettingExtenderModel>
{
    private readonly ICacheMessage _cache;
    private readonly IMessageManagementService _ManageService;

    public MessageController(ICacheMessage cache, IMessageManagementService ManageService)
    {
        _cache = cache;
        _ManageService = ManageService;
    }

    [HttpGet("Index")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Index()
    {
        await _ManageService.SaveMessagesToStorage();
        return Ok();
    }


    [HttpGet("Get Caches")]
    [ProducesResponseType(typeof(OkListResult<MessageRs>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAllCacheNotifs(CancellationToken cancellation = default(CancellationToken))
    {
        var notif = await _cache.GetAllMessages();
        return Ok(notif.ToList());
    }


    [HttpGet("SendNotif")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> CheckNotifs(CancellationToken ct = default(CancellationToken))
    {
        await _ManageService.SendMessages();
        return Ok();
    }

    [HttpPost("SaveNotif")]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(OkListResult<MessageVM>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> SendNotifAsync([FromBody] IEnumerable<MessageVM> entities, CancellationToken cancellationToken = default)
    {
        var result = await _cache.AddMessage(entities);
        return Ok(result);
    }

}
