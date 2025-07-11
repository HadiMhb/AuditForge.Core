namespace AuditForge.Configuration;

/// <summary>
/// Global configuration options for AuditForge.
/// </summary>
public class AuditOptions
{
    /// <summary>
    /// Enables or disables audit tracking.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Enables additional internal logging (for debugging).
    /// </summary>
    public bool EnableLogging { get; set; } = false;

    /// <summary>
    /// If true, sensitive properties will be masked.
    /// </summary>
    public bool MaskSensitiveData { get; set; } = true;

    /// <summary>
    /// Optional application or service name to record in audit logs.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Default configuration applied to all entities if not overridden.
    /// </summary>
    public EntityAuditOptions GlobalConfig { get; set; } = new();

    /// <summary>
    /// Per-entity audit configuration overrides.
    /// </summary>
    public Dictionary<Type, EntityAuditOptions> EntityConfigurations { get; set; } = new();
}
