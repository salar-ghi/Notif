namespace Domain.Entities.Base;

public abstract class EntityBase 
{
    protected EntityBase() { }

    //public virtual TId Id { get; set; }
    //public abstract string TableName { get; }

    protected List<DomainValidationError> _ValidationErrors { get; set; } = new List<DomainValidationError>();
    public IEnumerable<DomainValidationError> ValidationErrors => _ValidationErrors;

}
