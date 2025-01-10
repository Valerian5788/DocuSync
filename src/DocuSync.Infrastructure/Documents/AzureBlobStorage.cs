using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using DocuSync.Application.Services.Interfaces;
using Azure.Storage;

namespace DocuSync.Infrastructure.Documents
{
    public class AzureBlobStorage : IDocumentStorage
    {
        private readonly BlobServiceClient _blobService;
        private readonly string _containerName;
        private BlobContainerClient _containerClient;

        public AzureBlobStorage(IConfiguration configuration)
        {
            var connectionString = configuration["Azure:Storage:ConnectionString"]
                ?? throw new InvalidOperationException("Storage connection string not configured");

            _containerName = "temp-documents";
            _blobService = new BlobServiceClient(connectionString);
        }

        public async Task InitializeAsync()
        {
            // Create container if it doesn't exist
            _containerClient = _blobService.GetBlobContainerClient(_containerName);
            await _containerClient.CreateIfNotExistsAsync();

            // Set up lifecycle management (1 hour retention)
            await _containerClient.SetMetadataAsync(new Dictionary<string, string>
            {
                { "RetentionPolicy", "1hour" }
            });
        }

        public async Task<string> UploadAsync(
            Stream content,
            string filename,
            Guid clientId,
            Guid requirementId,
            CancellationToken cancellation = default)
                {
                    try
                    {
                        var blobName = $"{clientId}/{requirementId}/{filename}";
                        var blobClient = _containerClient.GetBlobClient(blobName);

                        await blobClient.UploadAsync(content, new BlobUploadOptions
                        {
                            TransferValidation = new UploadTransferValidationOptions { ChecksumAlgorithm = StorageChecksumAlgorithm.MD5 },
                            Metadata = new Dictionary<string, string>
                    {
                        { "ClientId", clientId.ToString() },
                        { "RequirementId", requirementId.ToString() },
                        { "OriginalFilename", filename }
                    }
                }, cancellation);

                return blobName;
            }
            catch (Exception ex)
            {
                throw new DocumentUploadException("Failed to upload document", ex);
            }
        }

        public async Task<string> GetTemporaryUrlAsync(
            string blobId,
            CancellationToken cancellation = default)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(blobId);

                // Generate SAS token valid for 15 minutes
                var sasBuilder = new BlobSasBuilder
                {
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15),
                    BlobContainerName = _containerName,
                    BlobName = blobId,
                    Resource = "b",
                    Protocol = SasProtocol.Https
                };
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                return blobClient.GenerateSasUri(sasBuilder).ToString();
            }
            catch (Exception ex)
            {
                throw new DocumentUploadException("Failed to generate temporary URL", ex);
            }
        }
    }
}