using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infrastructure.Services.EntityFramework;

public class NotifService : INotifService
{
    #region Definition & Ctor

    private readonly NotifContext _context;
    public NotifService(NotifContext context)
    {
        _context = context;
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


    public async Task<Notif> CreateNotifAsync(CreateNotifRq entity, CancellationToken ct = default(CancellationToken))
    {
        //Convert model to notif  ??????????????????
        ////////var items = await _context.Notifs.AddAsync(entity, ct);
        ////////await _context.SaveChangesAsync();

        ////////var res = await _context.Recipients.AddRangeAsync(entity.Recipients);

        //foreach (var Recipient in entity.Recipients)
        //{

        //    var res = await _context.AddAsync(, ct);
        //}

        //var itemId = items
        //return items.Entity;
        throw new NotImplementedException();


    }

    public async Task CreateNotifAsync(IEnumerable<CreateNotifRq> entities, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _context.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

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
