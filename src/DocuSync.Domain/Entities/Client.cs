using DocuSync.Domain.Common;
using DocuSync.Domain.Entities.Enums;

namespace DocuSync.Domain.Entities
{
    public class Client : Entity
    {
        public string Name { get; private set; }
        public string HorusEmail { get; private set; }
        public ClientStatus Status { get; private set; }

        // For EF
        private Client() { }

        public Client(string name, string horusEmail)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            if (string.IsNullOrWhiteSpace(horusEmail))
                throw new ArgumentException("Horus email is required", nameof(horusEmail));

            if (!IsValidEmail(horusEmail))
                throw new ArgumentException("Invalid email format", nameof(horusEmail));

            Name = name;
            HorusEmail = horusEmail;
            Status = ClientStatus.Active;
        }

        public void Deactivate()
        {
            if (Status == ClientStatus.Inactive)
                return;

            Status = ClientStatus.Inactive;
            UpdateAuditFields("system"); // Will be replaced with actual user later
        }

        public void Activate()
        {
            if (Status == ClientStatus.Active)
                return;

            Status = ClientStatus.Active;
            UpdateAuditFields("system"); // Will be replaced with actual user later
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name is required", nameof(newName));

            if (Name == newName)
                return;

            Name = newName;
            UpdateAuditFields("system"); // Will be replaced with actual user later
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}