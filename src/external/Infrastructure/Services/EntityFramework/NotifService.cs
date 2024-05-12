namespace Infrastructure.Services.EntityFramework;

public class NotifService : CRUDService<Notif>, INotifService
{
    #region Definition & Ctor

    private readonly IMapper _mapper;
    private readonly INotifSender _sender;
    private readonly ICacheMessage _cache;
    public NotifService(IMapper mapper, INotifSender sender, ICacheMessage cache)
    {
        _mapper = mapper;
        _sender = sender;
        _cache = cache;
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
        }while (saveFailed);
        
    }


    public async Task<Notif> SaveNotifAsync(NotifRq entity, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            // 1- first of all save messages to InMemoryCache or redis

            // 2- and simultanouesly do all actions that affect on performance
            // such as cuncurrency and parall processing and asynchronous programming ????????????????

            // 3- then run a background job that save all messages from InMemoryCache or redis to sql server

            // 3- then hangfire run a job that Delete all messages that were saved into sql server

            // 4- hangfire run a job that check database that when would send notifications


            // 5- Then call all those notifications and check by which method should Send (strategy design pattern)

            // 6- call type of provider then attemp to send notifs. (SMS / Email / Message Brocker -> rabbitmq, redisBus, kafka)

            // 7- do some actions for message persistency

            // 8- report the result of sending notifications.

            // 9- communication of project with other services and projects (rest- > sync / message brocker -> async)

            var notif = _mapper.Map<Notif>(entity);




            //???????????????????????????
            //var items = await _context.Notifs.AddAsync(notif, ct);
            //await _context.SaveChangesAsync();
            return notif;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task SaveNotifAsync(IEnumerable<NotifRq> entities, CancellationToken cancellationToken = default(CancellationToken))
    {
        var cache = await _cache.AddMessage(entities);

        var notif = _mapper.Map<IEnumerable<Notif>>(entities);

        //await _context.AddRangeAsync(entities);
        //await _context.SaveChangesAsync();
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
        var provider = await _unitOfWork.DbContext.Providers.Where(z => z.IsEnabled == true).FirstOrDefaultAsync();

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

    public async Task<bool> CacheNotifAsync(IEnumerable<NotifRq> entities, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _cache.AddMessage(entities);
        return result;
    }

    public async Task<IEnumerable<NotifRs>> GetAllNotifAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var notif = await _cache.GetAllMessages();
        //var notifResponse = _mapper.Map<IEnumerable<NotifRs>>(notif);
        return notif;
    }

    #endregion






    // **********************************?????????????????????
}
