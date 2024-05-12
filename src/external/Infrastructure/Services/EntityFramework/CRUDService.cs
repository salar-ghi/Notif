using Application.Models;
using Domain.Abstractions;
using Infrastructure.Abstractions.Persistence;

namespace Infrastructure.Services.EntityFramework;

public class CRUDService<T> : ICRUDService<T> where T : EntityBase, new()
{
    public UnitOfWork _unitOfWork { get; set; }
    protected readonly IGuard _guard;
    protected readonly IMapper _mapper;
    protected readonly ApplicationSettingExtenderModel _applicationSetting;


    public CRUDService()
    {
        _unitOfWork = (UnitOfWork)ServiceLocator.GetService<IUnitOfWork>();
        _applicationSetting = ServiceLocator.GetService<ApplicationSettingExtenderModel>();
        _guard = ServiceLocator.GetService<IGuard>();
        _mapper = ServiceLocator.GetService<IMapper>();
    }

    public virtual IQueryable<T> GetQuery()
    {
        return _unitOfWork.DbContext.Set<T>().AsNoTracking().AsQueryable();
    }

    public virtual IQueryable<TEntity> GetQueryAs<TEntity>()
    {
        return _unitOfWork.DbContext.Set<T>().AsNoTracking().OfType<TEntity>().AsQueryable();
    }


    public virtual IQueryable<T> GetQueryAsTracking()
    {
        return _unitOfWork.DbContext.Set<T>().AsQueryable();
    }

    public virtual async Task<T> Create(T entity)
    {
        ValidateEntity(entity);

        return _unitOfWork.DbContext.Set<T>().Add(entity).Entity;
    }

    public virtual async Task Create(ICollection<T> entities)
    {
        foreach (var entity in entities)
            ValidateEntity(entity);

        await _unitOfWork.DbContext.Set<T>().AddRangeAsync(entities);
    }

    public virtual async Task<T> Update(T entity)
    {
        ValidateEntity(entity);

        return _unitOfWork.DbContext.Set<T>().Update(entity).Entity;
    }

    public virtual async Task Update(ICollection<T> entities)
    {
        foreach (var entity in entities)
            ValidateEntity(entity);

        _unitOfWork.DbContext.Set<T>().UpdateRange(entities);
    }

    public virtual async Task Delete(T entity)
    {
        _unitOfWork.DbContext.Entry(entity).State = EntityState.Deleted;

        //UnitOfWork.DbContext.Set<T>().Remove(entity);
    }

    public virtual async Task Delete(ICollection<T> entities)
    {
        foreach (var entity in entities)
        {
            _unitOfWork.DbContext.Entry(entity).State = EntityState.Deleted;
        }

        //UnitOfWork.DbContext.Set<T>().RemoveRange(entities);
    }

    public virtual IQueryable<T> FromSql(string query)
    {
        return _unitOfWork.DbContext.Set<T>().FromSqlRaw(query).AsNoTracking();
    }

    public virtual IQueryable<TEntity> FromSql<TEntity>(string query) where TEntity : class
    {
        return _unitOfWork.DbContext.Set<TEntity>().FromSqlRaw(query).AsNoTracking();
    }

    public async Task SaveChanges(DateTime? useTrackableDateTime = null) =>
        await _unitOfWork.SaveChanges(useTrackableDateTime);

    public async Task SaveChanges(Func<Task> func, bool ignoreSaveChanges = true) =>
        await _unitOfWork.SaveChanges(func, ignoreSaveChanges);

    public async Task<TEntity> SaveChanges<TEntity>(Func<Task<TEntity>> func, bool ignoreSaveChanges = true) =>
        await _unitOfWork.SaveChanges<TEntity>(func, ignoreSaveChanges);

    protected void ValidateEntity(T entity)
    {
        if (entity is IValidate validatingEntity)
        {
            validatingEntity.Validate();
            _guard.Assert(validatingEntity.ValidationErrors.Count() < 1, Core.Enums.ExceptionCodeEnum.InvalidInput, "Domain validation failed.");
        }
    }

}
