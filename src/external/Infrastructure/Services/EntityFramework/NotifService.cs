﻿namespace Infrastructure.Services.EntityFramework;

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


    public async Task<Notif> CreateNotifAsync(CreateNotifRq entity)
    {
        //var items = await _context
        await _context.SaveChangesAsync();
        throw new NotImplementedException();
    }

    public Task CreateNotifAsync(ICollection<CreateNotifRq> entities)
    {
        throw new NotImplementedException();
    }

    public Task<Notif> UpdateNotifAsync(Notif entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNotifAsync(ICollection<Notif> entities)
    {
        throw new NotImplementedException();
    }




    #endregion
}
