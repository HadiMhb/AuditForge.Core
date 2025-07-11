namespace AuditForge.Application.Interfaces;

/// <summary>
/// Represents a change in a single property of an entity.
/// </summary>
public interface IPropertyChange
{
    /// <summary>
    /// The name of the property that changed.
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// The old value before the change.
    /// </summary>
    string? OldValue { get; set; }

    /// <summary>
    /// The new value after the change.
    /// </summary>
    string? NewValue { get; set; }
}
