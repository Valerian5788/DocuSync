using DocuSync.Domain.Common;
using DocuSync.Domain.Entities.Enums;

namespace DocuSync.Domain.Entities
{
    public class Requirement : Entity
    {
        public Guid ClientId { get; private set; }
        public DocumentType DocumentType { get; private set; }
        public DateTime DueDate { get; private set; }
        public RequirementStatus Status { get; private set; }

        public string BlobId { get; private set; }
        public DateTimeOffset? UploadedAt { get; private set; }

        // Navigation properties (for EF)
        private Client Client { get; set; }
        public Guid DocumentTypeId { get; private set; }

        // For EF Core
        private Requirement() { }

        public Requirement(Guid clientId, Guid documentTypeId, DateTime dueDate)
        {
            ValidateIds(clientId, documentTypeId);
            ValidateDueDate(dueDate);

            ClientId = clientId;
            DocumentTypeId = documentTypeId;
            DueDate = dueDate.Date; // Store date portion only
            Status = RequirementStatus.Pending;
        }

        public void MarkAsReceived()
        {
            ValidateStatusTransition(RequirementStatus.Received);
            Status = RequirementStatus.Received;
            UpdateAuditFields("system");
        }

        public void MarkAsValidated()
        {
            ValidateStatusTransition(RequirementStatus.Validated);
            Status = RequirementStatus.Validated;
            UpdateAuditFields("system");
        }

        public void MarkAsCompleted()
        {
            ValidateStatusTransition(RequirementStatus.Completed);
            Status = RequirementStatus.Completed;
            UpdateAuditFields("system");
        }

        public void MarkAsOverdue()
        {
            if (Status == RequirementStatus.Completed || Status == RequirementStatus.Cancelled)
                return;

            if (DateTime.UtcNow.Date > DueDate)
            {
                Status = RequirementStatus.Overdue;
                UpdateAuditFields("system");
            }
        }

        public void Cancel()
        {
            if (Status == RequirementStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed requirement");

            Status = RequirementStatus.Cancelled;
            UpdateAuditFields("system");
        }

        public void UpdateDueDate(DateTime newDueDate)
        {
            ValidateDueDate(newDueDate);

            if (Status == RequirementStatus.Completed || Status == RequirementStatus.Cancelled)
                throw new InvalidOperationException("Cannot update due date of completed or cancelled requirement");

            DueDate = newDueDate.Date;

            // Check if still overdue after date update
            if (Status == RequirementStatus.Overdue && DueDate >= DateTime.UtcNow.Date)
            {
                Status = RequirementStatus.Pending;
            }

            UpdateAuditFields("system");
        }

        private void ValidateIds(Guid clientId, Guid documentTypeId)
        {
            if (clientId == Guid.Empty)
                throw new ArgumentException("Client ID is required", nameof(clientId));

            if (documentTypeId == Guid.Empty)
                throw new ArgumentException("Document Type ID is required", nameof(documentTypeId));
        }

        private void ValidateDueDate(DateTime dueDate)
        {
            if (dueDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Due date cannot be in the past", nameof(dueDate));
        }

        private void ValidateStatusTransition(RequirementStatus newStatus)
        {
            if (Status == RequirementStatus.Completed)
                throw new InvalidOperationException("Cannot change status of completed requirement");

            if (Status == RequirementStatus.Cancelled)
                throw new InvalidOperationException("Cannot change status of cancelled requirement");

        }

        public void AttachDocument(string blobId)
        {
            if (string.IsNullOrEmpty(blobId))
                throw new ArgumentException("Blob ID is required", nameof(blobId));

            BlobId = blobId;
            UploadedAt = DateTimeOffset.UtcNow;
            MarkAsReceived();
        }

        public void RemoveDocument()
        {
            BlobId = null;
            UploadedAt = null;
            Status = RequirementStatus.Pending;
            UpdateAuditFields("system");
        }
    }
}