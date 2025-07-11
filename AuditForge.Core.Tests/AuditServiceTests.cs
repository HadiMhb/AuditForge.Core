using AuditForge.Application.Services;
using AuditForge.Application.Interfaces;
using AuditForge.Core.Application.Interfaces;
using AuditForge.Core.Domain.Enums;
using AuditForge.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using AuditForge.Core.Domain.Entities;
using AuditForge.Core.Domain.Attributes;

/// <summary>
/// Unit tests for the AuditService to verify behavior of tracking, saving, and retrieving audit entries.
/// </summary>
public class AuditServiceTests
{
    private readonly Mock<IAuditRepository> _repositoryMock;
    private readonly Mock<IUserProvider> _userProviderMock;
    private readonly Mock<IAuditEntryFactory> _factoryMock;
    private readonly AuditOptions _options;
    private readonly AuditService _service;

    public AuditServiceTests()
    {
        _repositoryMock = new Mock<IAuditRepository>();
        _userProviderMock = new Mock<IUserProvider>();
        _factoryMock = new Mock<IAuditEntryFactory>();

        _options = new AuditOptions
        {
            IsEnabled = true,
            GlobalConfig = new EntityAuditOptions()
        };

        var optionsWrapper = Options.Create(_options);

        _service = new AuditService(
            _repositoryMock.Object,
            _userProviderMock.Object,
            optionsWrapper,
            _factoryMock.Object
        );
    }

    private class TestEntity
    {
        [AuditKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    /// <summary>
    /// Ensures that Track adds a valid audit entry when configuration allows tracking.
    /// </summary>
    [Fact]
    public void Track_ShouldAddEntry_WhenConfigIsValid()
    {
        // Arrange
        var entity = new TestEntity { Id = 42, Name = "Test" };
        var userId = "user1";
        var expectedEntry = new AuditEntry(typeof(TestEntity));

        _userProviderMock.Setup(u => u.GetUserId()).Returns(userId);
        _factoryMock
            .Setup(f => f.CreateEntry(entity, AuditOperationType.Insert, "42", userId))
            .Returns(expectedEntry);

        // Act
        _service.Track(entity, AuditOperationType.Insert);

        // Assert
        var result = _service.GetPendingEntries();
        Assert.Single(result);
        Assert.Equal(expectedEntry, result.First());
    }

    /// <summary>
    /// Ensures that Track does not add an entry if auditing is disabled globally.
    /// </summary>
    [Fact]
    public void Track_ShouldDoNothing_WhenAuditIsDisabled()
    {
        // Arrange
        _options.IsEnabled = false;
        var service = new AuditService(
            _repositoryMock.Object,
            _userProviderMock.Object,
            Options.Create(_options),
            _factoryMock.Object);

        var entity = new TestEntity { Id = 1, Name = "NoTrack" };

        // Act
        service.Track(entity, AuditOperationType.Insert);

        // Assert
        Assert.Empty(service.GetPendingEntries());
    }

    /// <summary>
    /// Ensures that GetPendingEntries returns the entries tracked so far.
    /// </summary>
    [Fact]
    public void GetPendingEntries_ShouldReturnTrackedEntries()
    {
        var entity = new TestEntity { Id = 1, Name = "Test" };
        var auditEntry = new AuditEntry(typeof(TestEntity));

        _userProviderMock.Setup(u => u.GetUserId()).Returns("x");
        _factoryMock.Setup(f => f.CreateEntry(entity, AuditOperationType.Insert, "1", "x"))
            .Returns(auditEntry);

        _service.Track(entity, AuditOperationType.Insert);

        var entries = _service.GetPendingEntries();

        Assert.Single(entries);
        Assert.Equal(auditEntry, entries.First());
    }

    /// <summary>
    /// Ensures that SaveChangesAsync invokes SaveAsync and triggers BeforeSave and AfterSave events.
    /// </summary>
    [Fact]
    public async Task SaveChangesAsync_ShouldSaveEntries_AndTriggerEvents()
    {
        var entity = new TestEntity { Id = 1, Name = "SaveTest" };
        var entry = new AuditEntry(typeof(TestEntity));

        _userProviderMock.Setup(u => u.GetUserId()).Returns("x");
        _factoryMock.Setup(f => f.CreateEntry(entity, AuditOperationType.Update, "1", "x"))
            .Returns(entry);

        _service.Track(entity, AuditOperationType.Update);

        bool beforeSaveCalled = false;
        bool afterSaveCalled = false;

        _service.BeforeSave += async e =>
        {
            beforeSaveCalled = true;
            Assert.Equal(entry, e);
            await Task.CompletedTask;
        };

        _service.AfterSave += async e =>
        {
            afterSaveCalled = true;
            Assert.Equal(entry, e);
            await Task.CompletedTask;
        };

        // Act
        await _service.SaveChangesAsync();

        // Assert
        _repositoryMock.Verify(r => r.SaveAsync(entry, It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(beforeSaveCalled);
        Assert.True(afterSaveCalled);
        Assert.Empty(_service.GetPendingEntries()); // Make sure entries are cleared
    }
}
