using Domain.Entities;

namespace Infrastructure.Services.EntityFramework;

public class MessageLogService : CRUDService<MessageLog>,  IMessageLogService

{
    #region Definition & Ctor
    private readonly ILogger<MessageLogService> _logger;
    private readonly IMapper _mapper;

    public MessageLogService(ILogger<MessageLogService> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    
    #endregion

    #region Definition

    public async Task<MessageLog> SaveMessagLogAsync(MessageLog entity, CancellationToken ct)
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

    public async Task<bool> SaveMessagLogAsync(ICollection<MessageLog> entity, CancellationToken ct)
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

    public async Task<MessageLog> GetMessagLog(long Id)
    {
        try
        {
            //var lll = await _unitOfWork.DbContext.NotifLog
            //    .AsSplitQuery()
            //    .Include(s => s.Provider)
            //    .ThenInclude(s => new Provider { Id = s.Id, Name= s.Name, JsonConfig = s.JsonConfig })
            //    .Where(z => z.NotifId == Id)
            //    .Select(c => new NotifLog { Id = c.Id, ProviderId = c.ProviderId })
            //    .AsNoTracking()
            //    .ToListAsync()
            //    .ConfigureAwait(false);

            var logItem = await base.GetQuery()
                .Where(x => x.MessageId == Id)
                .AsNoTracking()
                .Select(j => new MessageLog
                {
                    Id = j.Id,
                    ProviderId = j.ProviderId,
                })
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
            return logItem;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }


    public async Task MarkMessagLogAsFailed(MessageLog log, CancellationToken ct)
    {
        try
        {
            log.SentAt = DateTime.UtcNow;
            log.FailureReason = "failed to send notif";
            var @event = await base.Update(log);
            await _unitOfWork.SaveChanges(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }


    public async Task MarkMessagLogAsSuccess(MessageLog log, CancellationToken ct)
    {
        try
        {
            log.Success = true;
            log.SentAt = DateTime.UtcNow;
            var @event = await base.Update(log);
            await _unitOfWork.SaveChanges(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    #endregion

}
