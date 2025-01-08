using DocuSync.Domain.Identity;
using DocuSync.Infrastructure.Data;
using DocuSync.Infrastructure.Identity.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUser
    {
        private readonly IUserClaimsAccessor _claimsAccessor;
        private readonly DocuSyncDbContext _dbContext;

        public CurrentUserService(
            IUserClaimsAccessor claimsAccessor,
            DocuSyncDbContext dbContext)
        {
            _claimsAccessor = claimsAccessor;
            _dbContext = dbContext;
        }

        public string Id => _claimsAccessor.UserId;

        public string Email => _claimsAccessor.Email;

        public bool IsAuthenticated => _claimsAccessor.IsAuthenticated;

        public bool IsAdmin => _claimsAccessor.Roles.Contains(UserRole.Administrator);

        public async Task<bool> CanAccessClientAsync(Guid clientId)
        {
            if (!IsAuthenticated)
                return false;

            if (IsAdmin)
                return true;

            return await _dbContext.ClientUsers
                .AnyAsync(cu => cu.ClientId == clientId
                    && cu.AzureAdUserId == Id);
        }
    }
}
