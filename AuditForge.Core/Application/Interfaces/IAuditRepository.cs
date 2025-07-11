using AuditForge.Application.Interfaces;
using AuditForge.Core.Domain.Entities;

namespace AuditForge.Core.Application.Interfaces
{
    /// <summary>
    /// Represents a contract for persisting audit logs.
    /// </summary>
    public interface IAuditRepository
    {
        /// <summary>
        /// Persists the given audit entry.
        /// </summary>
        /// <param name="entry">The audit entry to persist.</param>
        Task SaveAsync(IAuditEntry entry, CancellationToken cancellationToken = default);
    }
}
