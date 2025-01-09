using DocuSync.Application.Services;
using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Tests.Services
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _mockRepository;
        private readonly ClientService _service;

        public ClientServiceTests()
        {
            _mockRepository = new Mock<IClientRepository>();
            _service = new ClientService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllClients_WhenClientsExist()
        {
            // Arrange
            var clients = new List<Client>
        {
            new Client("Test1", "test1@horus.be"),
            new Client("Test2", "test2@horus.be")
        };
            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(clients);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFailure_WhenEmailExists()
        {
            // Arrange
            var email = "test@horus.be";
            _mockRepository.Setup(r => r.ExistsAsync(email))
                .ReturnsAsync(true);

            // Act
            var result = await _service.CreateAsync("Test", email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("already exists", result.Error);
        }

        [Fact]
        public async Task CreateAsync_CreatesClient_WhenValid()
        {
            // Arrange
            var name = "Test";
            var email = "test@horus.be";
            _mockRepository.Setup(r => r.ExistsAsync(email))
                .ReturnsAsync(false);

            // Act
            var result = await _service.CreateAsync(name, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data.Name);
            Assert.Equal(email, result.Data.HorusEmail);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFailure_WhenClientNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Client)null);

            // Act
            var result = await _service.UpdateAsync(id, "Test", "test@horus.be", ClientStatus.Active);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("not found", result.Error);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenClientExists()
        {
            // Arrange
            var client = new Client("Test", "test@horus.be");
            _mockRepository.Setup(r => r.GetByIdAsync(client.Id))
                .ReturnsAsync(client);

            // Act
            var result = await _service.DeleteAsync(client.Id);

            // Assert
            Assert.True(result.IsSuccess);
            _mockRepository.Verify(r => r.DeleteAsync(client), Times.Once);
        }

        private Client CreateValidClient()
        => new Client("Test Client", "test@horus.be");

        [Theory]
        [InlineData("", "test@horus.be", "Name is required")]
        [InlineData("  ", "test@horus.be", "Name is required")]
        [InlineData("Test", "", "Email is required")]
        [InlineData("Test", "  ", "Email is required")]
        [InlineData("Test", "invalid-email", "Invalid email format")]
        public async Task CreateAsync_ReturnsFailure_WhenInputInvalid(string name, string email, string expectedError)
        {
            // Act
            var result = await _service.CreateAsync(name, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedError, result.Error);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesStatus_WhenChangingToInactive()
        {
            // Arrange
            var client = CreateValidClient();
            _mockRepository.Setup(r => r.GetByIdAsync(client.Id))
                .ReturnsAsync(client);

            // Act
            var result = await _service.UpdateAsync(client.Id, "New Name", client.HorusEmail, ClientStatus.Inactive);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ClientStatus.Inactive, result.Data.Status);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_PreservesOriginalEmail_WhenOnlyUpdatingName()
        {
            // Arrange
            var client = CreateValidClient();
            var originalEmail = client.HorusEmail;
            _mockRepository.Setup(r => r.GetByIdAsync(client.Id))
                .ReturnsAsync(client);

            // Act
            var result = await _service.UpdateAsync(client.Id, "New Name", originalEmail, client.Status);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("New Name", result.Data.Name);
            Assert.Equal(originalEmail, result.Data.HorusEmail);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFailure_WhenClientNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Client)null);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("not found", result.Error);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Client>()), Times.Never);
        }
    }
}
