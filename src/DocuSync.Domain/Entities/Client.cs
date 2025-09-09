using DocuSync.Domain.Common;
using DocuSync.Domain.Entities.Enums;

namespace DocuSync.Domain.Entities
{
    /// <summary>
    /// Client aggregate root representing an organization using the document management system.
    /// Implements Domain-Driven Design principles with rich domain behavior and business rule enforcement.
    /// </summary>
    public class Client : Entity
    {
        public string Name { get; private set; }
        public string HorusEmail { get; private set; }
        public ClientStatus Status { get; private set; }

        // Private parameterless constructor for EF Core
        // EF Core requires this for entity instantiation during database queries
        private Client() { }

        /// <summary>
        /// Creates a new client with required business validation.
        /// This constructor enforces business invariants at creation time rather than relying on external validation.
        /// </summary>
        /// <param name="name">Client organization name - must be unique per business rules</param>
        /// <param name="horusEmail">Primary contact email - used for critical compliance notifications</param>
        public Client(string name, string horusEmail)
        {
            // Domain validation: Fail fast principle - validate inputs at domain boundary
            // This ensures invalid entities can never exist in our system
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            if (string.IsNullOrWhiteSpace(horusEmail))
                throw new ArgumentException("Horus email is required", nameof(horusEmail));

            // Email validation at domain level ensures data integrity
            // Using custom validation instead of data attributes for domain independence
            if (!IsValidEmail(horusEmail))
                throw new ArgumentException("Invalid email format", nameof(horusEmail));

            Name = name;
            HorusEmail = horusEmail;
            // Business rule: New clients are always active by default
            // This supports the common workflow where clients are created and immediately usable
            Status = ClientStatus.Active;
        }

        /// <summary>
        /// Deactivates client while preserving audit trail.
        /// Business rule: Inactive clients cannot create new documents but retain historical data for compliance.
        /// </summary>
        public void Deactivate()
        {
            // Idempotent operation - safe to call multiple times
            // This prevents unnecessary audit log entries and database updates
            if (Status == ClientStatus.Inactive)
                return;

            Status = ClientStatus.Inactive;
            // TODO: Replace "system" with actual user context when authentication is fully implemented
            // This maintains audit trail showing who performed the action and when
            UpdateAuditFields("system");
        }

        /// <summary>
        /// Reactivates client for continued document management.
        /// Business rule: Only inactive clients can be reactivated.
        /// </summary>
        public void Activate()
        {
            // Idempotent operation pattern - prevents unnecessary state changes
            if (Status == ClientStatus.Active)
                return;

            Status = ClientStatus.Active;
            // Audit trail maintained for compliance and debugging purposes
            UpdateAuditFields("system");
        }

        /// <summary>
        /// Updates client name with validation and audit trail.
        /// Business rule: Name changes must be tracked for compliance and user experience.
        /// </summary>
        /// <param name="newName">New organization name</param>
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name is required", nameof(newName));

            // Performance optimization: Skip update if name hasn't actually changed
            // This prevents unnecessary database writes and audit log entries
            if (Name == newName)
                return;

            Name = newName;
            UpdateAuditFields("system");
        }

        /// <summary>
        /// Validates email format using .NET's built-in validation.
        /// Private method encapsulates validation logic and can be easily unit tested.
        /// Performance consideration: Uses try-catch for validation as it's more reliable
        /// than regex for edge cases in email validation.
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if email format is valid</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                // Using System.Net.Mail.MailAddress for validation provides:
                // 1. Built-in .NET validation logic (more reliable than custom regex)
                // 2. Handles edge cases and international characters
                // 3. Consistent with other .NET email validation throughout the system
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                // Any exception during creation indicates invalid email format
                // This approach is more reliable than string parsing
                return false;
            }
        }
    }
}