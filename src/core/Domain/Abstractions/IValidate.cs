using Core.DomainValidation.Models;

namespace Domain.Abstractions;

public interface IValidate
{
    void Validate();
    IEnumerable<DomainValidationError> ValidationErrors { get; }
}
