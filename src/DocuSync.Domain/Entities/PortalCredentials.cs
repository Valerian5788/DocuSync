using DocuSync.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Domain.Entities
{
    public class PortalCredentials : Entity
    {
        public Guid ClientId { get; private set; }
        public string PortalType { get; private set; }
        public string Username { get; private set; }
        public byte[] EncryptedPassword { get; private set; }
        public virtual Client Client { get; private set; }

        private PortalCredentials() { } 

        public PortalCredentials(Guid clientId, string portalType, string username, byte[] encryptedPassword)
        {
            ClientId = clientId;
            PortalType = portalType;
            Username = username;
            EncryptedPassword = encryptedPassword;
        }
    }
}
