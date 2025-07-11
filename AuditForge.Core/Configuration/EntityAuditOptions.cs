namespace AuditForge.Configuration;

/// <summary>
/// Per-entity audit configuration.
/// </summary>
public class EntityAuditOptions
{
    /// <summary>
    /// If true, this entity will be excluded from auditing.
    /// </summary>
    public bool IgnoreEntity { get; set; } = false;

    /// <summary>
    /// List of property names to ignore when tracking changes.
    /// </summary>
    public List<string> IgnoredProperties { get; set; } = new();

    /// <summary>
    /// List of property names to mask in audit logs.
    /// </summary>
    public List<string> SensitiveProperties { get; set; } = new();

    /// <summary>
    /// If true, unchanged properties will be tracked as well.
    /// </summary>
    public bool TrackUnchangedProperties { get; set; } = false;
}
