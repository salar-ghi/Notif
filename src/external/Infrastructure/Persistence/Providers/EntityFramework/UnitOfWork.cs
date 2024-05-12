using Domain.Abstractions;
using Infrastructure.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.Providers.EntityFramework;

public class UnitOfWork : IUnitOfWork
{
    private bool _ignoreSaveChanges = false;

    public NotifContext DbContext { get; set; }
    public UnitOfWork(NotifContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task SaveChanges(DateTime? trackableDateTime = null, CancellationToken cancellationToken = default)
    {
        if (_ignoreSaveChanges) return;

        var dateTime = trackableDateTime ?? DateTime.UtcNow;
        ProcessTrackable(dateTime);
        
        if(DbContext.ChangeTracker.HasChanges())
            await DbContext.SaveChangesAsync();

        DbContext.ChangeTracker.Clear();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default) =>
            await SaveChanges(null, cancellationToken);

    public async Task SaveChanges(Func<Task> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default) =>
        await SaveChanges(async () =>
        {
            await func();
            return 0;
        }, ignoreSaveChanges);

    public async Task<T> SaveChanges<T>(Func<Task<T>> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default) =>
        await SaveChanges(async transaction => await func(), null, ignoreSaveChanges, cancellationToken);

    private async Task<T> SaveChanges<T>(Func<IDbContextTransaction, Task<T>> func, IDbContextTransaction transaction, bool ignoreSaveChanges = true,
                                         CancellationToken cancellationToken = default)
    {
        try
        {
            _ignoreSaveChanges = ignoreSaveChanges;
            var result = await func(transaction);
            _ignoreSaveChanges = false;

            //DetachValueObjects();

            await SaveChanges(cancellationToken);

            return result;
        }
        finally
        {
            _ignoreSaveChanges = false;
        }
    }

    public virtual async Task<int> ExecuteSql(string query, params object[] parameters)
    {
        return await DbContext.Database.ExecuteSqlRawAsync(query, parameters);
    }

    //public async Task<T> DoItTransactional<T>(Func<IDbContextTransaction, Task<T>> func, IsolationLevel isolationLevel, bool ignoreSaveChanges = true)
    //{
    //    await using var transaction = await DbContext.Database.BeginTransactionAsync(isolationLevel);

    //    try
    //    {
    //        var result = await SaveChanges(func, transaction, ignoreSaveChanges);
    //        await transaction.CommitAsync();
    //        return result;
    //    }
    //    catch (Exception e)
    //    {
    //        await transaction.RollbackAsync();
    //        throw;
    //    }
    //}

    //public async Task<T> DoItTransactional<T>(Func<IDbContextTransaction, Task<T>> func, bool ignoreSaveChanges = true) => await DoItTransactional(func, IsolationLevel.ReadCommitted, ignoreSaveChanges);

    //public async Task DoItTransactional(Func<IDbContextTransaction, Task> func, bool ignoreSaveChanges = true) =>
    //    await DoItTransactional(async (transaction) =>
    //    {
    //        await func(transaction);
    //        return 0;
    //    }, ignoreSaveChanges);

    //public async Task DoItTransactional(Func<Task> func, bool ignoreSaveChanges = true) => await DoItTransactional(async _ => await func(), ignoreSaveChanges);

    //public async Task<T> DoItTransactional<T>(Func<Task<T>> func, bool ignoreSaveChanges = true) => await DoItTransactional(async _ => await func(), ignoreSaveChanges);

    #region Dispose
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion




    private void ProcessTrackable(DateTime trackableDateTime)
    {
        var entries = DbContext.ChangeTracker.Entries().Where(x =>
            x.Entity is ITrackable && (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList();
        foreach (var entry in entries)
        {
            var trackable = entry.Entity as ITrackable;

            var principalId = 0; // AuthPrincipalService.GetCurrentPrincipalIdIfLogin();

            switch (entry.State)
            {
                case EntityState.Modified:
                    trackable.ModifiedAt = trackableDateTime;
                    trackable.ModifiedById = principalId;
                    break;

                case EntityState.Added:
                    trackable.CreatedAt = trackableDateTime;
                    trackable.CreatedById = principalId;
                    trackable.ModifiedAt = null;
                    break;
            }
        }
    }
}