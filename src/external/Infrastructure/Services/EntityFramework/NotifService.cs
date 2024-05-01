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


    public async Task<Notif> CreateNotifAsync(CreateNotifRq notifEntity, CancellationToken ct = default(CancellationToken))
    {
        var notif  = _mapper.Map<Notif>(notifEntity);

        var items = await _context.Notifs.AddAsync(notif, ct);
        await _context.SaveChangesAsync();

        return notif;

        ////foreach (var child in notifEntity.Recipients)
        ////{
        ////    child.NotifId = notif.Id;
        ////}
        ////var recipList = notifEntity.Recipients;
        ////var recip = _mapper.Map<ICollection<Recipient>?>(recipList);

        ////await _context.Recipients?.AddRangeAsync(recip, ct);
        ////await _context.SaveChangesAsync();

        //return items.Entity;
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
