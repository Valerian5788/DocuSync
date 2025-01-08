using DocuSync.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Domain.Entities
{
    public class ClientUser : Entity
    {
        public Guid ClientId { get; private set; }
        public string AzureAdUserId { get; private set; }
        public virtual Client Client { get; private set; }

        private ClientUser() { } // For EF Core

        public ClientUser(Guid clientId, string azureAdUserId)
        {
            if (clientId == Guid.Empty)
                throw new ArgumentException("Client ID is required", nameof(clientId));

            if (string.IsNullOrWhiteSpace(azureAdUserId))
                throw new ArgumentException("Azure AD User ID is required", nameof(azureAdUserId));

            ClientId = clientId;
            AzureAdUserId = azureAdUserId;
        }
    }
}
