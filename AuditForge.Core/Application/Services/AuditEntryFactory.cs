using AuditForge.Application.Interfaces;
using AuditForge.Core.Domain.Entities;
using AuditForge.Core.Domain.Enums;

namespace AuditForge.Application.Services;

/// <summary>
/// Default factory for creating audit entries and property changes.
/// </summary>
public class AuditEntryFactory : IAuditEntryFactory
{
    public IAuditEntry CreateEntry(object entity, AuditOperationType operationType, string entityId, string? userId)
    {
        return new AuditEntry(entity)
        {
            EntityName = entity.GetType().Name,
            EntityId = entityId,
            OperationType = operationType,
            UserId = userId,
            Changes = new List<IPropertyChange>()
        };
    }

    public IPropertyChange CreatePropertyChange(string propertyName, string? oldValue, string? newValue)
    {
        return new PropertyChange
        {
            PropertyName = propertyName,
            OldValue = oldValue,
            NewValue = newValue
        };
    }
}
