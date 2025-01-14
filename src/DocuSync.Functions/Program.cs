using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Azure.Identity;
using DocuSync.Functions.Graph.Services;
using Microsoft.Extensions.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var options = new ClientSecretCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var credential = new ClientSecretCredential(
                config["Graph:TenantId"],
                config["Graph:ClientId"],
                config["Graph:ClientSecret"],
                options);

            return new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });
        });

        services.AddSingleton<IGraphMailService, GraphMailService>();
    })
    .Build();

host.Run();