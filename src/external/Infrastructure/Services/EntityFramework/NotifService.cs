using StackExchange.Redis;

namespace Infrastructure.Services.EntityFramework;

public class NotifService : CRUDService<Notif>, INotifService
{
    #region Definition & Ctor

    private readonly ILogger<NotifService> _logger;
    private readonly IMapper _mapper;
    private readonly INotifSender _sender;


    // ************* //
    private readonly IProviderService _provider;
    private readonly INotifLogService _notifLog;
    public NotifService(IMapper mapper, INotifSender sender, ILogger<NotifService> logger, IProviderService provider, INotifLogService notifLog)
    {
        _mapper = mapper;
        _sender = sender;
        _logger = logger;
        _provider = provider;
        _notifLog = notifLog;
    }

    #endregion

    #region Methods

    public async Task MarkNotificationsAsReadAsync(List<Notif> notifs, CancellationToken cancellationToken)
    {
        Parallel.ForEach(notifs, async notif =>
        {
            //var @event = await _unitOfWork.DbContext.Notifs.FindAsync(notif.Id);
            var @event = await base.Update(notif);
            @event.status = NotifStatus.Delivered;
            await _unitOfWork.SaveChanges(cancellationToken);
        });
        bool saveFailed;
        do
        {
            saveFailed = false;
            try
            {

                await _unitOfWork.SaveChanges(cancellationToken);
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

    public async Task SaveNotifAsync(IEnumerable<NotifRq> entities, CancellationToken cancellationToken = default(CancellationToken))
    {
        //var cache = await _cache.AddMessage(entities);

        var notif = _mapper.Map<ICollection<Notif>>(entities);
        await base.Create(notif);
        await _unitOfWork.DbContext.SaveChangesAsync();
    }

    //public async Task<List<bool>> CreateNotifAsync(IEnumerable<NotifVM> entities, CancellationToken cancellationToken = default(CancellationToken))
    //{
    //    try
    //    {
    //        ICollection<NotifLog> notifLogCol = new HashSet<NotifLog>();
    //        foreach (var entity in entities)
    //        {
    //            var notif = _mapper.Map<Notif>(entity);

    //            var data = await base.Create(notif);
    //            await _unitOfWork.DbContext.SaveChangesAsync();


    //            var provider = !string.IsNullOrEmpty(entity.ProviderName) ? _provider.GetSpecificProvider(entity.ProviderName) : _provider.GetRandomProvider(entity.ProviderName, entity.Type);
    //            var notLog = new NotifLog
    //            {
    //                NotifId =  data.Id,
    //                ProviderId = provider.Id,
    //            };
    //            notifLogCol.Add(notLog);
    //            await _notifLog.SaveNotifLogAsync(notifLogCol, cancellationToken);
    //        }
    //        // ??????????????????? start to remove saved items to storage from cache ***********
    //        // ??????????????????? start to remove saved items to storage from cache ***********
    //        // ??????????????????? start to remove saved items to storage from cache ***********
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex.Message, ex);
    //        throw;
    //    }

    //}


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

                    var resItem = await _sender.SendNotificationAsync(notif, message.ProviderName);
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
        return await _unitOfWork.DbContext.Notifs.Where(e => e.status != NotifStatus.Delivered).ToListAsync();
    }


    #endregion


    #region Cache

    //public async Task<bool> CacheNotifAsync(IEnumerable<NotifVM> entities, CancellationToken cancellationToken = default(CancellationToken))
    //{
    //    var result = await _cache.AddMessage(entities);
    //    return result;
    //}

    //public async Task<IEnumerable<NotifVM>> GetAllNotifAsync(CancellationToken cancellationToken = default(CancellationToken))
    //{
    //    var notif = await _cache.GetAllMessages();
    //    return notif;
    //}

    #endregion






    // **********************************?????????????????????
}
