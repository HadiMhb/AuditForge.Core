using AuditForge.Core.Domain.Entities;

namespace AuditForge.Core.Application.Interfaces;

/// <summary>
/// Provides functionality to track and persist audit entries.
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Adds an audit entry to the pending queue.
    /// </summary>
    /// <param name="entry">The audit entry to track.</param>
    void Track(AuditEntry entry);

    /// <summary>
    /// Saves all pending audit entries.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the list of currently tracked audit entries.
    /// </summary>
    /// <returns>A read-only list of audit entries.</returns>
    IReadOnlyList<AuditEntry> GetPendingEntries();

    /// <summary>
    /// Hook that runs before saving each audit entry.
    /// </summary>
    event Func<AuditEntry, Task>? BeforeSave;

    /// <summary>
    /// Hook that runs after saving each audit entry.
    /// </summary>
    event Func<AuditEntry, Task>? AfterSave;
}
