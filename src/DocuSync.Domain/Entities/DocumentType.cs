using DocuSync.Domain.Common;
using DocuSync.Domain.Entities.Enums;

namespace DocuSync.Domain.Entities
{
    public class DocumentType : Entity
    {
        private const int MaxNameLength = 100;
        private const int MaxDescriptionLength = 500;

        public string Name { get; private set; }
        public string Description { get; private set; }
        public DocumentFrequency Frequency { get; private set; }

        // For EF Core
        private DocumentType() { }

        public DocumentType(string name, DocumentFrequency frequency, string description = null)
        {
            ValidateName(name);
            ValidateDescription(description);

            Name = name;
            Frequency = frequency;
            Description = description;
        }

        public void UpdateName(string newName)
        {
            ValidateName(newName);

            if (Name == newName)
                return;

            Name = newName;
            UpdateAuditFields("system"); // Will be replaced with actual user
        }

        public void UpdateDescription(string newDescription)
        {
            ValidateDescription(newDescription);

            if (Description == newDescription)
                return;

            Description = newDescription;
            UpdateAuditFields("system"); // Will be replaced with actual user
        }

        public void UpdateFrequency(DocumentFrequency newFrequency)
        {
            if (Frequency == newFrequency)
                return;

            Frequency = newFrequency;
            UpdateAuditFields("system"); // Will be replaced with actual user
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            if (name.Length > MaxNameLength)
                throw new ArgumentException($"Name cannot exceed {MaxNameLength} characters", nameof(name));
        }

        private void ValidateDescription(string description)
        {
            if (description?.Length > MaxDescriptionLength)
                throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters", nameof(description));
        }
    }
}
