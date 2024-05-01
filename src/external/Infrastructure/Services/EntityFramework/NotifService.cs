using Hangfire;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infrastructure.Services.EntityFramework;

public class NotifService : INotifService
{
    #region Definition & Ctor

    private readonly NotifContext _context;
    private readonly IMapper _mapper;
    public NotifService(NotifContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task MarkNotificationsAsReadAsync(List<Notif> notifs)
    {
        foreach (var notif in notifs)
        {
            notif.status = NotifStatus.Delivered;
        }
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
            var notif = _mapper.Map<Notif>(entity);
            var items = await _context.Notifs.AddAsync(notif, ct);
            await _context.SaveChangesAsync();
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
            var job = BackgroundJob.Enqueue(() => SendNotificationAsync(entity));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //BackgroundJob.Enqueue(() => Console.WriteLine("Hangfire Triggered ????????????????????????????????????*********************** / How are you "));

    }

    private async Task SendNotificationAsync(Notif message)
    {
        // Code to send the notification to the recipient
        // ...



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
