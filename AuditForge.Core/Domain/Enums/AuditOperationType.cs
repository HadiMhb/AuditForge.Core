
namespace AuditForge.Core.Domain.Enums
{
    /// <summary>
    /// Indicates the type of operation that triggered the audit.
    /// </summary>
    public enum AuditOperationType
    {
        /// <summary>
        /// A new entity was inserted.
        /// </summary>
        Insert,

        /// <summary>
        /// An existing entity was updated.
        /// </summary>
        Update,

        /// <summary>
        /// An entity was deleted.
        /// </summary>
        Delete
    }
}
