using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;

namespace DocuSync.Domain.Tests.Entities
{
    public class RequirementTests
    {
        private readonly Guid _validClientId = Guid.NewGuid();
        private readonly Guid _validDocumentTypeId = Guid.NewGuid();
        private readonly DateTime _validDueDate = DateTime.UtcNow.Date.AddDays(1);

        [Fact]
        public void Constructor_WithValidInput_CreatesRequirement()
        {
            // Act
            var requirement = new Requirement(_validClientId, _validDocumentTypeId, _validDueDate);

            // Assert
            Assert.Equal(_validClientId, requirement.ClientId);
            Assert.Equal(_validDocumentTypeId, requirement.DocumentTypeId);
            Assert.Equal(_validDueDate.Date, requirement.DueDate);
            Assert.Equal(RequirementStatus.Pending, requirement.Status);
        }

        [Fact]
        public void Constructor_WithPastDueDate_ThrowsException()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.Date.AddDays(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Requirement(_validClientId, _validDocumentTypeId, pastDate));
        }

        [Fact]
        public void MarkAsReceived_WhenPending_UpdatesStatus()
        {
            // Arrange
            var requirement = new Requirement(_validClientId, _validDocumentTypeId, _validDueDate);

            // Act
            requirement.MarkAsReceived();

            // Assert
            Assert.Equal(RequirementStatus.Received, requirement.Status);
        }

        [Fact]
        public void MarkAsCompleted_WhenCancelled_ThrowsException()
        {
            // Arrange
            var requirement = new Requirement(_validClientId, _validDocumentTypeId, _validDueDate);
            requirement.Cancel();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => requirement.MarkAsCompleted());
        }

        [Fact]
        public void UpdateDueDate_WithValidDate_UpdatesDate()
        {
            // Arrange
            var requirement = new Requirement(_validClientId, _validDocumentTypeId, _validDueDate);
            var newDueDate = _validDueDate.AddDays(1);

            // Act
            requirement.UpdateDueDate(newDueDate);

            // Assert
            Assert.Equal(newDueDate.Date, requirement.DueDate);
        }

        [Fact]
        public void MarkAsOverdue_WhenPastDueDate_UpdatesStatus()
        {
            // Arrange
            var requirement = new Requirement(_validClientId, _validDocumentTypeId, DateTime.UtcNow.AddDays(1));

            // Use reflection to modify the private DueDate field to simulate past date
            typeof(Requirement).GetProperty("DueDate")
                .SetValue(requirement, DateTime.UtcNow.AddDays(-1).Date);

            // Act
            requirement.MarkAsOverdue();

            // Assert
            Assert.Equal(RequirementStatus.Overdue, requirement.Status);
        }
    }
}