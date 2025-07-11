using AuditForge.Application.Services;
using AuditForge.Core.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace AuditForge.Tests.Services
{
    /// <summary>
    /// Unit tests for the <see cref="AuditEntryFactory"/> class.
    /// </summary>
    public class AuditEntryFactoryTests
    {
        private readonly AuditEntryFactory _factory;

        public AuditEntryFactoryTests()
        {
            _factory = new AuditEntryFactory();
        }

        /// <summary>
        /// Ensures that CreateEntry creates a valid audit entry with the correct metadata.
        /// </summary>
        [Fact]
        public void CreateEntry_ShouldCreateAuditEntryWithCorrectValues()
        {
            // Arrange
            var entity = new { Id = 1, Name = "Test" };
            var operationType = AuditOperationType.Update;
            var entityId = "1";
            var userId = "test-user";

            // Act
            var result = _factory.CreateEntry(entity, operationType, entityId, userId);

            // Assert
            result.Should().NotBeNull();
            result.EntityId.Should().Be(entityId);
            result.EntityName.Should().Be(entity.GetType().Name);
            result.OperationType.Should().Be(operationType);
            result.UserId.Should().Be(userId);
            result.Changes.Should().BeEmpty();
        }

        /// <summary>
        /// Ensures that CreatePropertyChange creates a valid property change object with expected values.
        /// </summary>
        [Fact]
        public void CreatePropertyChange_ShouldReturnPropertyChangeWithCorrectValues()
        {
            // Arrange
            var propertyName = "Name";
            var oldValue = "Old";
            var newValue = "New";

            // Act
            var result = _factory.CreatePropertyChange(propertyName, oldValue, newValue);

            // Assert
            result.Should().NotBeNull();
            result.PropertyName.Should().Be(propertyName);
            result.OldValue.Should().Be(oldValue);
            result.NewValue.Should().Be(newValue);
        }
    }
}
