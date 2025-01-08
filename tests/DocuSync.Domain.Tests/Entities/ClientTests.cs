using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;

namespace DocuSync.Domain.Tests.Entities
{
    public class ClientTests
    {
        [Fact]
        public void Constructor_WithValidInput_CreatesClient()
        {
            // Arrange
            string name = "Test Client";
            string email = "test@horus.be";

            // Act
            var client = new Client(name, email);

            // Assert
            Assert.Equal(name, client.Name);
            Assert.Equal(email, client.HorusEmail);
            Assert.Equal(ClientStatus.Active, client.Status);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidName_ThrowsException(string invalidName)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Client(invalidName, "test@horus.be"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("invalid-email")]
        [InlineData("@horus.be")]
        public void Constructor_WithInvalidEmail_ThrowsException(string invalidEmail)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Client("Test Client", invalidEmail));
        }

        [Fact]
        public void Deactivate_WhenActive_ChangesStatusToInactive()
        {
            // Arrange
            var client = new Client("Test Client", "test@horus.be");

            // Act
            client.Deactivate();

            // Assert
            Assert.Equal(ClientStatus.Inactive, client.Status);
        }

        [Fact]
        public void Activate_WhenInactive_ChangesStatusToActive()
        {
            // Arrange
            var client = new Client("Test Client", "test@horus.be");
            client.Deactivate();

            // Act
            client.Activate();

            // Assert
            Assert.Equal(ClientStatus.Active, client.Status);
        }
    }
}