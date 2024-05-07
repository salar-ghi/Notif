using Domain.Entities;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Services.EntityFramework;

public class NotifService : INotifService
{
    #region Definition & Ctor

    private readonly NotifContext _context;
    private readonly IMapper _mapper;
    private readonly INotifSender _sender;
    private readonly IMemoryCache _cache;
    public NotifService(NotifContext context, IMapper mapper, INotifSender sender, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _sender = sender;
        _cache = cache;
    }

    #endregion

    #region Methods

    public async Task MarkNotificationsAsReadAsync(List<Notif> notifs)
    {
        Parallel.ForEach(notifs, async notif =>
        {
            var @event = await _context.Notifs.FindAsync(notif.Id);
            @event.status = NotifStatus.Delivered;
            await _context.SaveChangesAsync();
        });
        bool saveFailed;
        do
        {
            saveFailed = false;
            try
            {
                    
                await _context.SaveChangesAsync();
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


    public async Task<Notif> SaveNotifAsync(CreateNotifRq entity, CancellationToken ct = default(CancellationToken))
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

            string key = Guid.NewGuid().ToString(); // Generate unique key
            _cache.Set(key, notif, TimeSpan.FromMinutes(5));


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

    public async Task SaveNotifAsync(IEnumerable<CreateNotifRq> entities, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _context.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
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

    public async Task SendNotificationAsync(IEnumerable<CreateNotifRq> messages)
    {
        // Code to send the notification to the recipient
        // ...
        Parallel.ForEach(messages, async message =>
        {
            var notif = _mapper.Map<Notif>(message);
            switch (message.Type)
            {
                case NotifType.SMS:
                    var result = new SmsNotifSender();
                    var resItem = result.SendNotificationAsync(notif, message.ProviderName);
                    //_sender.SendNotificationAsync(message);
                    var @event = await _context.Notifs.FindAsync(notif.Id);
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
        return await _context.Notifs.Where(e => e.status != NotifStatus.Delivered).ToListAsync();
    }







    public Task<Notif> UpdateNotifAsync(Notif entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNotifAsync(IEnumerable<Notif> entities)
    {
        throw new NotImplementedException();
    }


    #endregion
}
