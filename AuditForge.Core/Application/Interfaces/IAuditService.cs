using AuditForge.Core.Domain.Entities;
using AuditForge.Core.Domain.Enums;

namespace AuditForge.Application.Interfaces
{
    /// <summary>
    /// Provides methods for tracking and saving audit entries.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Tracks changes to a given entity.
        /// </summary>
        /// <param name="entity">The entity being audited.</param>
        /// <param name="operation">The type of operation performed.</param>
        /// <param name="userId">Optional user identifier.</param>
        void Track(object entity, AuditOperationType operation, string? userId = null);

        /// <summary>
        /// Persists all pending audit entries.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the list of currently pending audit entries.
        /// </summary>
        IReadOnlyList<IAuditEntry> GetPendingEntries();

        /// <summary>
        /// Hook triggered before an audit entry is saved.
        /// </summary>
        event Func<IAuditEntry, Task>? BeforeSave;

        /// <summary>
        /// Hook triggered after an audit entry is saved.
        /// </summary>
        event Func<IAuditEntry, Task>? AfterSave;
    }
}
