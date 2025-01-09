using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using DocuSync.Infrastructure.Data;
using DocuSync.Infrastructure.Data.Repositories;
using DocuSync.Domain.Repositories;
using DocuSync.Infrastructure.Identity.Configuration;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using System;
using DocuSync.Infrastructure.Configuration;
using DocuSync.Domain.Identity;
using DocuSync.Infrastructure.Identity;
using DocuSync.Infrastructure.Identity.Interfaces;
using DocuSync.Infrastructure.Identity.Services;

namespace DocuSync.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocuSyncInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Validate parameters
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // Register options
            services.Configure<DocuSyncDbOptions>(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("DocuSync");
            });

            // Register DbContext
            services.AddDbContext<DocuSyncDbContext>((serviceProvider, options) =>
            {
                var dbOptions = serviceProvider
                    .GetRequiredService<IOptions<DocuSyncDbOptions>>()
                    .Value;

                options.UseSqlServer(dbOptions.ConnectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
            });

            // Register repositories
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            services.AddScoped<IRequirementRepository, RequirementRepository>();

            services.AddScoped<IUserClaimsAccessor, DesignTimeClaimsAccessor>();

            services.AddScoped<ICurrentUser, CurrentUserService>();

            // Add Identity Services
            services.AddDocuSyncIdentity(configuration);

            return services;
        }

        public static IServiceCollection AddDocuSyncIdentity(
    this IServiceCollection services,
    IConfiguration configuration)
        {
            var options = new IdentityOptions();
            configuration.GetSection(IdentityOptions.SectionName).Bind(options);
            options.Validate();

            services.Configure<IdentityOptions>(
                configuration.GetSection(IdentityOptions.SectionName));

            // Configure OpenID Connect options (if needed)
            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // Configure additional OpenID Connect options here if needed
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.UseTokenLifetime = true;
            });

            // Register the current user service
            services.AddScoped<ICurrentUser, CurrentUserService>();

            return services;
        }
    }
}