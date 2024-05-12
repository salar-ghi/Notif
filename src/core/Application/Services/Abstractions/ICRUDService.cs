namespace Application.Services.Abstractions;

public interface ICRUDService<T> where T : class
{
    IQueryable<T> GetQuery();
    IQueryable<TEntity> GetQueryAs<TEntity>();
    IQueryable<T> GetQueryAsTracking();
    Task<T> Create(T entity);
    Task Create(ICollection<T> entities);
    Task<T> Update(T entity);
    Task Update(ICollection<T> entities);
    Task Delete(T entity);
    Task Delete(ICollection<T> entities);
    IQueryable<T> FromSql(string query);
    IQueryable<TEntity> FromSql<TEntity>(string query) where TEntity : class;


    Task SaveChanges(DateTime? useTrackableDateTime = null);
    Task SaveChanges(Func<Task> func, bool ignoreSaveChanges = true);
    Task<TEntity> SaveChanges<TEntity>(Func<Task<TEntity>> func, bool ignoreSaveChanges = true);
}
