namespace AuditForge.Application.Interfaces;

using AuditForge.Core.Domain.Enums;

/// <summary>
/// Factory for creating audit entries and property changes.
/// </summary>
public interface IAuditEntryFactory
{
    /// <summary>
    /// Creates a new audit entry for the given entity.
    /// </summary>
    IAuditEntry CreateEntry(object entity, AuditOperationType operationType, string entityId, string? userId);

    /// <summary>
    /// Creates a property change representation.
    /// </summary>
    IPropertyChange CreatePropertyChange(string propertyName, string? oldValue, string? newValue);
}
