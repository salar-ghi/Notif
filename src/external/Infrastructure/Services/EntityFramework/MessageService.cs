namespace Infrastructure.Services.EntityFramework;

public class MessageService : CRUDService<Message>, IMessageService
{
    #region Definition & Ctor

    private readonly ILogger<MessageService> _logger;
    private readonly IMapper _mapper;
    private readonly ApplicationSettingExtenderModel _configuration;
    public int AttempValue { get; set; } = default(int);
    // ************* //

    public MessageService(IMapper mapper, ILogger<MessageService> logger, ApplicationSettingExtenderModel configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _configuration = configuration;
    }

    #endregion

    #region Methods

    public async Task<bool> MarkMessagesAsReadAsync(Message notif, CancellationToken ct)
    {
        try
        {
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    Message note = new Message
                    {
                        status = NotifStatus.Delivered,
                        Attemp = notif.Attemp + 1,
                        IsSent = true,
                        ModifiedAt = DateTime.UtcNow,
                    };
                    //_unitOfWork.DbContext.Notifs.Attach(notif);
                    //await base.Update(notif);

                    var testExecuteUpdate = _unitOfWork.DbContext.Message.Where(j => j.Id == notif.Id)
                        .ExecuteUpdate(b => b
                        .SetProperty(x => x.status, NotifStatus.Delivered)
                        .SetProperty(x => x.Attemp, notif.Attemp + 1)
                        .SetProperty(x => x.IsSent, true)
                        .SetProperty(x => x.ModifiedAt, DateTime.UtcNow)
                        );
                }
                //catch (Exception ex)
                catch (DbUpdateConcurrencyException ex)
                {
                    _unitOfWork.DbContext.Message.Entry(notif).Reload();
                    Message dto = new Message
                    {
                        status = NotifStatus.Delivered,
                        Attemp = notif.Attemp + 1,
                        IsSent = true,
                        ModifiedAt = DateTime.UtcNow,
                    };
                    _unitOfWork.DbContext.Message.Entry(notif).CurrentValues.SetValues(dto);
                    await _unitOfWork.DbContext.SaveChangesAsync(ct);
                }
            } while (saveFailed);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
    public async Task MarkMessagesAsReadAsync(List<Message> notifs, CancellationToken ct)
    {
        bool saveFailed;
        do
        {
            saveFailed = false;
            try
            {
                await Parallel.ForEachAsync(notifs, async (notif, ct) =>
                {
                    //var @event = await _unitOfWork.DbContext.Notifs.FindAsync(notif.Id);
                    notif.status = NotifStatus.Delivered;
                    notif.Attemp = notif.Attemp + 1;
                    notif.IsSent = true;
                    var @event = await base.Update(notif);
                });
                await _unitOfWork.SaveChanges(ct);
                //await _unitOfWork.SaveChanges(cancellationToken);
            }
            //catch (Exception ex)
            catch (DbUpdateConcurrencyException ex)
            {
                saveFailed = true;
                var entry = ex.Entries.SingleOrDefault();
                var databaseValues = entry.GetDatabaseValues();

                // - Update the original values with the database values and retry
                // - Throw an exception or return a response indicating the conflict
                entry.OriginalValues.SetValues(databaseValues);
            }
        } while (saveFailed);

    }

    public async Task MarkMessageAsFailedAttemp(Message notif, CancellationToken ct)
    {
        try
        {
            var ExecuteUpdate = _unitOfWork.DbContext.Message.Where(j => j.Id == notif.Id)
                .ExecuteUpdate(b => b
                .SetProperty(x => x.Attemp, notif.Attemp + 1)
                .SetProperty(x => x.ModifiedAt, DateTime.UtcNow));
        }
        catch (Exception ex)
        {
            _unitOfWork.DbContext.Message.Entry(notif).Reload();
            Message dto = new Message
            {
                Attemp = notif.Attemp + 1,
                IsSent = false,
                ModifiedAt = DateTime.UtcNow,
            };
            _unitOfWork.DbContext.Message.Entry(notif).CurrentValues.SetValues(dto);
            await _unitOfWork.DbContext.SaveChangesAsync(ct);
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    public async Task<IEnumerable<Message>> GetUnDeliveredAsync()
    {
        AttempValue = _configuration.Jobs.Attemp;
        var undeliverNotifs = await base.GetQuery()
            .Where(x => x.status == NotifStatus.waiting && x.Attemp < AttempValue)
            .AsNoTracking()
            .Select(z => new Message
            {
                Id = z.Id,
                Title = z.Title,
                Body = z.Body,
                Type = z.Type,
                MessageType = z.MessageType,
                Attemp = z.Attemp,
                ProviderID = z.ProviderID,
                status = z.status,
                Recipients = z.Recipients,
            })
            .ToListAsync()
            .ConfigureAwait(false);
        return undeliverNotifs;
    }

    public async Task<Message> SaveMessageAsync(MessageVM entity, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var notif = _mapper.Map<Message>(entity);
            var data = await base.Create(notif);
            await _unitOfWork.DbContext.SaveChangesAsync();
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    public async Task SaveMessageAsync(IEnumerable<MessageVM> entities, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var notif = _mapper.Map<ICollection<Message>>(entities);
            await base.Create(notif);
            await _unitOfWork.DbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    #endregion


    // **********************************?????????????????????
}
