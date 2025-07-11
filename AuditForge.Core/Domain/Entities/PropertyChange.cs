using AuditForge.Application.Interfaces;

namespace AuditForge.Core.Domain.Entities
{
    /// <summary>
    /// Represents a change made to a single property of an entity.
    /// </summary>
    public class PropertyChange : IPropertyChange
    {
        /// <summary>
        /// The name of the property that changed.
        /// </summary>
        public string PropertyName { get; set; } = default!;

        /// <summary>
        /// The value of the property before the change (as string).
        /// </summary>
        public string? OldValue { get; set; }

        /// <summary>
        /// The value of the property after the change (as string).
        /// </summary>
        public string? NewValue { get; set; }
    }
}
