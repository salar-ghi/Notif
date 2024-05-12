namespace Infrastructure.Abstractions.Persistence;

public interface IUnitOfWork : IDisposable
{
    NotifContext DbContext { get; }
    Task SaveChanges(DateTime? useTrackableDateTime = null, CancellationToken cancellationToken = default);
    Task SaveChanges(Func<Task> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default);
    Task<T> SaveChanges<T>(Func<Task<T>> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default);
    Task<int> ExecuteSql(string query, params object[] parameters);

    //Task<T> DoItTransactional<T>(Func<IDbContextTransaction, Task<T>> func, IsolationLevel isolationLevel, bool ignoreSaveChanges = true);
    //Task DoItTransactional(Func<Task> func, bool ignoreSaveChanges = true);
    //Task<T> DoItTransactional<T>(Func<Task<T>> func, bool ignoreSaveChanges = true);
    //Task<T> DoItTransactional<T>(Func<IDbContextTransaction, Task<T>> func, bool ignoreSaveChanges = true);
    //Task DoItTransactional(Func<IDbContextTransaction, Task> func, bool ignoreSaveChanges = true);
}
