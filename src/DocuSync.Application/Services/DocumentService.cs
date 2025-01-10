using DocuSync.Application.Services.Interfaces;
using DocuSync.Domain.Identity;
using DocuSync.Domain.Repositories;
using DocuSync.Application.Common.Exceptions;

namespace DocuSync.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentStorage _storage;
        private readonly IRequirementRepository _requirements;
        private readonly ICurrentUser _currentUser;

        public DocumentService(
            IDocumentStorage storage,
            IRequirementRepository requirements,
            ICurrentUser currentUser)
        {
            _storage = storage;
            _requirements = requirements;
            _currentUser = currentUser;
        }

        public async Task<DocumentUploadResult> UploadForRequirementAsync(
    Guid requirementId,
    Stream content,
    string filename,
    CancellationToken cancellation = default)
        {
            var requirement = await _requirements.GetByIdAsync(requirementId, cancellation);
            if (requirement == null)
                return new DocumentUploadResult(false, null, "Requirement not found");

            try
            {
                var blobId = await _storage.UploadAsync(
                    content,
                    filename,
                    requirement.ClientId,
                    requirementId,
                    cancellation);

                requirement.AttachDocument(blobId);
                await _requirements.UpdateAsync(requirement, cancellation);

                return new DocumentUploadResult(true, blobId);
            }
            catch (DocumentUploadException ex)
            {
                return new DocumentUploadResult(false, null, ex.Message);
            }
        }

        public async Task<DocumentStatus> GetStatusAsync(
            Guid requirementId,
            CancellationToken cancellation = default)
        {
            var requirement = await _requirements.GetByIdAsync(requirementId, cancellation);
            if (requirement == null)
                throw new NotFoundException("Requirement not found");

            return new DocumentStatus(
                requirement.Id,
                requirement.BlobId != null,
                requirement.UploadedAt,
                requirement.Status.ToString());
        }
    }
}
