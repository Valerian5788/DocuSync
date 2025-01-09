using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Authorization.Requirements
{
    public class ClientAccessRequirement : IAuthorizationRequirement
    {
        public Guid ClientId { get; }

        public ClientAccessRequirement(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}
