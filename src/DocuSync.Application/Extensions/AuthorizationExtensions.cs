using DocuSync.Application.Authorization.Constants;
using DocuSync.Application.Authorization.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using DocuSync.Application.Authorization.Requirements;
namespace DocuSync.Application.Extensions
{
    public static class AuthorizationExtensions
    {

        public static IServiceCollection AddDocuSyncAuthorization(this IServiceCollection services)
        {
            // Register authorization handlers
            services.AddScoped<IAuthorizationHandler, AdminAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ClientAccessAuthorizationHandler>();

            // Add authorization policies
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(Policies.IsAdmin, policy =>
                {
                    policy.Requirements.Add(new AdminRequirement());
                });

                options.AddPolicy(Policies.CanAccessClient, policy =>
                {
                    policy.Requirements.Add(new ClientAccessRequirement(Guid.Empty));
                });
            });

            return services;
        }
    }
}
