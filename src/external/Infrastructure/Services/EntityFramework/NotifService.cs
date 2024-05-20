using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework;

public class NotifService : CRUDService<Notif>, INotifService
{
    #region Definition & Ctor

    private readonly ILogger<NotifService> _logger;
    private readonly IMapper _mapper;
    private readonly ApplicationSettingExtenderModel _configuration;
    public int AttempValue { get; set; } = default(int);
    // ************* //

    public NotifService(IMapper mapper, ILogger<NotifService> logger, ApplicationSettingExtenderModel configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _configuration = configuration;
    }

    #endregion

    #region Methods

    public async Task<bool> MarkNotificationsAsReadAsync(Notif notif, CancellationToken ct)
    {
        try
        {
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    Notif note = new Notif
                    {
                        status = NotifStatus.Delivered,
                        Attemp = notif.Attemp + 1,
                        IsSent = true,
                    };
                    //_unitOfWork.DbContext.Notifs.Attach(notif);
                    //await base.Update(notif);

                    var testExecuteUpdate = _unitOfWork.DbContext.Notifs.Where(j => j.Id == notif.Id)
                        .ExecuteUpdate(b => b.SetProperty(x => x.status, NotifStatus.Delivered).SetProperty(x => x.Attemp, notif.Attemp + 1));

                    //_unitOfWork.DbContext.Notifs.Entry(notif).CurrentValues.SetValues(note);
                    //_unitOfWork.DbContext.Notifs.Entry(notif).State = EntityState.Modified;

                    await _unitOfWork.DbContext.SaveChangesAsync(ct).ConfigureAwait(false);
                }
                //catch (Exception ex)
                catch (DbUpdateConcurrencyException ex)
                {
                    _unitOfWork.DbContext.Notifs.Entry(notif).Reload();
                    Notif dto = new Notif
                    {
                        status = NotifStatus.Delivered,
                        Attemp = notif.Attemp + 1,
                        IsSent = true,
                    };
                    _unitOfWork.DbContext.Notifs.Entry(notif).CurrentValues.SetValues(dto);
                    await _unitOfWork.DbContext.SaveChangesAsync(ct);

                    //saveFailed = true;
                    //var entry = ex.Entries.SingleOrDefault();
                    //var databaseValues = entry.GetDatabaseValues();
                    //entry.OriginalValues.SetValues(databaseValues);
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

    public async Task MarkNotificationAsFailedAttemp(Notif notif, CancellationToken ct)
    {
        try
        {
            notif.Attemp = notif.Attemp + 1;
            var @event = await base.Update(notif);
            await _unitOfWork.SaveChanges(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
    public async Task MarkNotificationsAsReadAsync(List<Notif> notifs, CancellationToken ct)
    {
        bool saveFailed;
        do
        {
            saveFailed = false;
            try
            {
                await Parallel.ForEachAsync(notifs, async (notif, ct )=>
                {
                    //var @event = await _unitOfWork.DbContext.Notifs.FindAsync(notif.Id);
                    notif.status = NotifStatus.Delivered;
                    notif.Attemp = notif.Attemp+1;
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


    public async Task<Notif> SaveNotifAsync(NotifVM entity, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var notif = _mapper.Map<Notif>(entity);
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

    public async Task SaveNotifAsync(IEnumerable<NotifVM> entities, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var notif = _mapper.Map<ICollection<Notif>>(entities);
            await base.Create(notif);
            await _unitOfWork.DbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }


    public async Task ScheduleNotificationAsync(Notif entity, CancellationToken ct)
    {
        try
        {
            //var job = BackgroundJob.Enqueue(() => SendNotificationAsync(entity));
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //BackgroundJob.Enqueue(() => Console.WriteLine("Hangfire Triggered ????????????????????????????????????*********************** / How are you "));

    }

    public async Task SendNotificationAsync(IEnumerable<NotifRq> messages)
    {
        // Code to send the notification to the recipient
        // ...        
        var provider = await _unitOfWork.DbContext.Provider.Where(z => z.IsEnabled == true).FirstOrDefaultAsync();

        Parallel.ForEach(messages, async message =>
        {
            var notif = _mapper.Map<Notif>(message);
            switch (message.Type)
            {
                case NotifType.SMS:
                    //var provider = await _context.Providers.Where(z => z.IsEnabled == true).FirstOrDefaultAsync();


                    //_sender.SendNotificationAsync(message);
                    var @event = await _unitOfWork.DbContext.Notifs.FindAsync(notif.Id);
                    @event.status = NotifStatus.Delivered;
                    break;
                case NotifType.Email:
                    break;
                case NotifType.MessageBrocker:
                    break;
                case NotifType.Signal:
                    break;
                default:
                    break;
            }
        });


        await Task.CompletedTask;
    }


    public async Task<IEnumerable<Notif>> GetUnDeliveredAsync()
    {
        AttempValue = _configuration.Jobs.Attemp;
        var undeliverNotifs = await base.GetQuery()            
            .Where(x => x.status == NotifStatus.waiting && x.Attemp < AttempValue)
            .AsNoTracking()
            .Select(z => new Notif
            {
                Id = z.Id,
                Title = z.Title,
                Message = z.Message,
                Type = z.Type,
                MessageType = z.MessageType,
                Attemp = z.Attemp,
                ProviderID = z.ProviderID,
                status = z.status,
                Recipients = z.Recipients,
            })
            .ToListAsync()
            .ConfigureAwait(false);

        //.Where(e => e.status != NotifStatus.Delivered && e.status != NotifStatus.failed).ToListAsync();
        return undeliverNotifs;
    }


    #endregion


    // **********************************?????????????????????
}
