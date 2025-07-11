
namespace AuditForge.Core.Domain.Attributes;

/// <summary>
/// Marks a property as the primary key for audit tracking.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class AuditKeyAttribute : Attribute
{
}