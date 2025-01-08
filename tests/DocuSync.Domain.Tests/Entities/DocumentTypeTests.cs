using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;

namespace DocuSync.Domain.Tests.Entities
{
    public class DocumentTypeTests
    {
        [Fact]
        public void Constructor_WithValidInput_CreatesDocumentType()
        {
            // Arrange
            string name = "Invoice";
            string description = "Standard invoice document";
            var frequency = DocumentFrequency.Monthly;

            // Act
            var docType = new DocumentType(name, frequency, description);

            // Assert
            Assert.Equal(name, docType.Name);
            Assert.Equal(description, docType.Description);
            Assert.Equal(frequency, docType.Frequency);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidName_ThrowsException(string invalidName)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new DocumentType(invalidName, DocumentFrequency.Monthly));
        }

        [Fact]
        public void Constructor_WithTooLongName_ThrowsException()
        {
            // Arrange
            string tooLongName = new string('x', 101);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new DocumentType(tooLongName, DocumentFrequency.Monthly));
        }

        [Fact]
        public void UpdateName_WithValidName_ChangesName()
        {
            // Arrange
            var docType = new DocumentType("Old Name", DocumentFrequency.Monthly);
            string newName = "New Name";

            // Act
            docType.UpdateName(newName);

            // Assert
            Assert.Equal(newName, docType.Name);
        }

        [Fact]
        public void UpdateFrequency_WithDifferentFrequency_ChangesFrequency()
        {
            // Arrange
            var docType = new DocumentType("Test", DocumentFrequency.Monthly);

            // Act
            docType.UpdateFrequency(DocumentFrequency.Quarterly);

            // Assert
            Assert.Equal(DocumentFrequency.Quarterly, docType.Frequency);
        }
    }
}