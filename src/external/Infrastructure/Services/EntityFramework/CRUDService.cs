using AutoMapper;
using Core;
using Core.Abstractions;
using Infrastructure.Persistence.Providers.EntityFramework;

namespace Infrastructure.Services.EntityFramework;

public class CRUDService<T> where T : EntityBase, new()
{
    public UnitOfWork _unitOfWork { get; set; }
    protected readonly IGuard _guard;
    protected readonly IMapper _mapper;

    public CRUDService()
    {
        _unitOfWork = (UnitOfWork)ServiceLocator.GetService<UnitOfWork>();
        _guard = ServiceLocator.GetService<IGuard>();
        _mapper = ServiceLocator.GetService<IMapper>();
    }

    public virtual IQueryable<T> GetQuery()
    {
        return _unitOfWork._context.Set<T>().AsNoTracking().AsQueryable();
    }

    public virtual IQueryable<TEntity> GetQueryAs<TEntity>()
    {
        return _unitOfWork._context.Set<T>().AsNoTracking().OfType<TEntity>().AsQueryable();
    }




}
