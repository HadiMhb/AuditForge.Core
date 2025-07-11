using AuditForge.Application.Interfaces;
using AuditForge.Configuration;
using AuditForge.Core.Application.Interfaces;
using AuditForge.Core.Domain.Attributes;
using AuditForge.Core.Domain.Enums;
using Microsoft.Extensions.Options;

namespace AuditForge.Application.Services
{
    /// <summary>
    /// Default implementation of the audit service.
    /// </summary>
    public class AuditService : IAuditService
    {
        private readonly List<IAuditEntry> _pendingEntries = new();
        private readonly IAuditRepository _repository;
        private readonly IUserProvider _userProvider;
        private readonly AuditOptions _options;
        private readonly IAuditEntryFactory _factory;

        /// <inheritdoc/>
        public event Func<IAuditEntry, Task>? BeforeSave;

        /// <inheritdoc/>
        public event Func<IAuditEntry, Task>? AfterSave;

        /// <summary>
        /// Creates an instance of the audit service.
        /// </summary>
        public AuditService(
            IAuditRepository repository,
            IUserProvider userProvider,
            IOptions<AuditOptions> options,
            IAuditEntryFactory factory)
        {
            _repository = repository;
            _userProvider = userProvider;
            _options = options.Value;
            _factory = factory;
        }

        /// <summary>
        /// Retrieves the primary key value of the given entity using the [AuditId] attribute.
        /// If the attribute is not found, returns an empty string.
        /// </summary>
        /// <param name="entity">The entity from which to extract the ID.</param>
        /// <returns>The string representation of the entity's ID, or an empty string if not found.</returns>
        private string GetEntityId(object entity)
        {
            var type = entity.GetType();

            var keyProp = type
                .GetProperties()
                .FirstOrDefault(p => Attribute.IsDefined(p, typeof(AuditKeyAttribute)));

            if (keyProp == null)
                throw new InvalidOperationException($"No property marked with [AuditKey] found on entity '{type.Name}'.");

            var value = keyProp.GetValue(entity);
            return value?.ToString() ?? throw new InvalidOperationException("Key property value is null.");
        }

        /// <inheritdoc/>
        public void Track(object entity, AuditOperationType operation, string? userId = null)
        {
            if (!_options.IsEnabled || entity == null)
                return;

            var entityType = entity.GetType();

            var config = _options.EntityConfigurations.TryGetValue(entityType, out var customConfig)
                ? customConfig
                : _options.GlobalConfig;

            if (config.IgnoreEntity)
                return;

            var entityId = GetEntityId(entity);

            var auditEntry = _factory.CreateEntry(entity, operation, entityId, userId ?? _userProvider.GetUserId());

            auditEntry.Changes.RemoveAll(pc => config.IgnoredProperties.Contains(pc.PropertyName));

            if (!config.TrackUnchangedProperties)
            {
                auditEntry.Changes.RemoveAll(pc => Equals(pc.OldValue, pc.NewValue));
            }

            _pendingEntries.Add(auditEntry);
        }


        /// <inheritdoc/>
        public IReadOnlyList<IAuditEntry> GetPendingEntries()
        {
            return _pendingEntries.AsReadOnly();
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in _pendingEntries)
            {
                if (BeforeSave != null)
                    await BeforeSave.Invoke(entry);

                await _repository.SaveAsync(entry, cancellationToken);

                if (AfterSave != null)
                    await AfterSave.Invoke(entry);
            }

            _pendingEntries.Clear();
        }
    }
}
