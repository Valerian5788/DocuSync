using DocuSync.Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DocuSync.Web.Identity
{
    public class ClaimsUserAccessor : IUserClaimsAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ??
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            string.Empty;

        public string Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ??
            _httpContextAccessor.HttpContext?.User?.FindFirst("preferred_username")?.Value ??
            string.Empty;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public IEnumerable<string> Roles
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return Array.Empty<string>();

                var roleClaims = user.FindAll(ClaimTypes.Role).ToList();
                if (!roleClaims.Any())
                {
                    // Check for Azure AD roles claim
                    var azureRoles = user.FindFirst("roles")?.Value;
                    if (!string.IsNullOrEmpty(azureRoles))
                    {
                        return azureRoles.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    }
                }

                return roleClaims.Select(c => c.Value).ToArray();
            }
        }
    }
}