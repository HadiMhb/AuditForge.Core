using AuditForge.Core.Domain.Entities;
using AuditForge.Core.Domain.Enums;

namespace AuditForge.Application.Interfaces;

/// <summary>
/// Represents a single audit log entry.
/// </summary>
public interface IAuditEntry
{
    /// <summary>
    /// The CLR type of the entity being audited.
    /// </summary>
    public Type? EntityType { get; }

    /// <summary>
    /// The name of the entity that was changed (e.g., "User", "Invoice").
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    /// The unique identifier of the entity instance (e.g., primary key).
    /// </summary>
    public string EntityId { get; set; }

    /// <summary>
    /// The type of operation performed on the entity (Insert, Update, Delete).
    /// </summary>
    public AuditOperationType OperationType { get; set; }

    /// <summary>
    /// The ID of the user who performed the operation (if available).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// The UTC timestamp of when the operation occurred.
    /// </summary>
    public DateTime PerformedAt { get; set; }

    /// <summary>
    /// A list of individual property changes made in this operation.
    /// </summary>
    public List<IPropertyChange> Changes { get; set; }
}
