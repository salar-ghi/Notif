namespace Infrastructure.Services.EntityFramework;

public class NotifLogService : CRUDService<NotifLog>,  INotifLogService
{
    #region Definition & Ctor
    private readonly ILogger<NotifLogService> _logger;
    private readonly IMapper _mapper;

    public NotifLogService(ILogger<NotifLogService> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    
    #endregion

    #region Definition

    public Task<NotifLog> SaveNotifLogAsync(NotifLog entity, CancellationToken ct)
    {
        try
        {

            return Task.FromResult<NotifLog>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }



    #endregion

}
