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

    public async Task<NotifLog> SaveNotifLogAsync(NotifLog entity, CancellationToken ct)
    {
        try
        {
            var result = await base.Create(entity).ConfigureAwait(false);
            await _unitOfWork.DbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    public async Task<bool> SaveNotifLogAsync(ICollection<NotifLog> entity, CancellationToken ct)
    {
        try
        {
            await base.Create(entity);
            await _unitOfWork.DbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }



    #endregion

}
