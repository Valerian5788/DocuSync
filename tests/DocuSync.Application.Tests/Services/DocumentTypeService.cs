using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocuSync.Application.Services;
using DocuSync.Application.Services.Interfaces;

namespace DocuSync.Application.Tests.Services
{
    public class DocumentTypeServiceTests
    {
        private readonly Mock<IDocumentTypeRepository> _mockRepository;
        private readonly DocumentTypeService _service;

        public DocumentTypeServiceTests()
        {
            _mockRepository = new Mock<IDocumentTypeRepository>();
            _service = new DocumentTypeService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllDocumentTypes()
        {
            // Arrange
            var types = new List<DocumentType>
        {
            new DocumentType("Invoice", DocumentFrequency.Monthly),
            new DocumentType("Annual Report", DocumentFrequency.Annual)
        };
            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(types);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task CreateAsync_CreatesDocumentType_WhenValid()
        {
            // Arrange
            var name = "Invoice";
            var frequency = DocumentFrequency.Monthly;

            // Act
            var result = await _service.CreateAsync(name, frequency);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data.Name);
            Assert.Equal(frequency, result.Data.Frequency);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFailure_WhenTypeInUse()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.IsInUseAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("in use", result.Error);
        }
    }
}
