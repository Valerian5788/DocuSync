using DocuSync.Infrastructure.Identity.Interfaces;

namespace DocuSync.Infrastructure.Identity.Services
{
    public class DesignTimeClaimsAccessor : IUserClaimsAccessor
    {
        public string UserId => "DesignTime";
        public string Email => "design@time.local";
        public bool IsAuthenticated => true;
        public IEnumerable<string> Roles => new[] { "Administrator" };
    }
}