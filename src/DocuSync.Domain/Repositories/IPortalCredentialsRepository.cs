using DocuSync.Domain.Common;
using DocuSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Domain.Repositories
{
    public interface IPortalCredentialsRepository : IRepository<PortalCredentials>
    {
        Task<PortalCredentials> GetByClientAsync(Guid clientId, string portalType);
        Task<bool> ExistsForClientAsync(Guid clientId, string portalType);
    }
}
