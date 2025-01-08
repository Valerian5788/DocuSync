using System;

namespace DocuSync.Infrastructure.Identity.Configuration
{
    public class IdentityOptions
    {
        public const string SectionName = "AzureAd";

        public string Instance { get; set; } = "https://login.microsoftonline.com/";
        public string Domain { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; } = "/signin-oidc";
        public string SignedOutCallbackPath { get; set; } = "/signout-oidc";

        public void Validate()
        {
            if (string.IsNullOrEmpty(Domain))
                throw new InvalidOperationException($"{nameof(Domain)} is required");
            if (string.IsNullOrEmpty(TenantId))
                throw new InvalidOperationException($"{nameof(TenantId)} is required");
            if (string.IsNullOrEmpty(ClientId))
                throw new InvalidOperationException($"{nameof(ClientId)} is required");
            if (string.IsNullOrEmpty(ClientSecret))
                throw new InvalidOperationException($"{nameof(ClientSecret)} is required");
        }
    }
}