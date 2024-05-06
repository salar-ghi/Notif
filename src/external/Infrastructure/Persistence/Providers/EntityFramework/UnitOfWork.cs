namespace Infrastructure.Persistence.Providers.EntityFramework;

public class UnitOfWork //: IUnitOfWork
{
    private bool _ignoreSaveChanges = false;

    public NotifContext _context { get; set; }
    public UnitOfWork(NotifContext notifContext)
    {
        _context = notifContext;
    }







}