using AuditForge.Core.Application.Interfaces;
using AuditForge.Core.Domain.Entities;

namespace AuditForge.Core.Application.Services;

/// <summary>
/// Default implementation of the audit service that manages tracking and saving audit entries.
/// </summary>
public class AuditService : IAuditService
{
    private readonly IAuditRepository _repository;
    private readonly List<AuditEntry> _pendingEntries = new();

    public event Func<AuditEntry, Task>? BeforeSave;
    public event Func<AuditEntry, Task>? AfterSave;

    public AuditService(IAuditRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public void Track(AuditEntry entry)
    {
        _pendingEntries.Add(entry);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in _pendingEntries.ToList()) // Copy to avoid modification issues
        {
            if (BeforeSave is not null)
                await BeforeSave.Invoke(entry);

            await _repository.SaveAsync(entry);

            if (AfterSave is not null)
                await AfterSave.Invoke(entry);
        }

        _pendingEntries.Clear();
    }

    /// <inheritdoc />
    public IReadOnlyList<AuditEntry> GetPendingEntries() => _pendingEntries.AsReadOnly();
}
