using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Domain.Identity
{
    public interface ICurrentUser
    {
        string Id { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        Task<bool> CanAccessClientAsync(Guid clientId);
    }
}
