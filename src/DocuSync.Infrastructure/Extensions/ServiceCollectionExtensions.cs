using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DocuSync.Infrastructure.Data;
using DocuSync.Infrastructure.Configuration;
using DocuSync.Infrastructure.Data.Repositories;
using DocuSync.Domain.Repositories;

namespace DocuSync.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for registering infrastructure services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds DocuSync infrastructure services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration instance</param>
        /// <returns>The service collection for chaining</returns>
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

            return services;
        }
    }
}