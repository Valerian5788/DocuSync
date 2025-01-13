using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Microsoft.Graph;

namespace DocuSync.Functions.Graph.Functions
{
    public class GraphSubscriptionRenewalFunction
    {
        private readonly GraphServiceClient _graphClient;
        private readonly string _notificationUrl;
        private readonly ILogger<GraphSubscriptionRenewalFunction> _logger;

        public GraphSubscriptionRenewalFunction(
            GraphServiceClient graphClient,
            IConfiguration configuration,
            ILogger<GraphSubscriptionRenewalFunction> logger)
        {
            _graphClient = graphClient;
            _notificationUrl = configuration["Graph:NotificationUrl"];
            _logger = logger;
        }

        [Function("GraphSubscriptionRenewal")]
        public async Task RunAsync([TimerTrigger("0 0 */2 * * *")] TimerInfo timer)
        {
            try
            {
                // Get existing subscriptions
                var subscriptions = await _graphClient.Subscriptions
                    .GetAsync();

                foreach (var subscription in subscriptions.Value)
                {
                    // Renew if expiring soon (within 1 day)
                    if (subscription.ExpirationDateTime <= DateTimeOffset.UtcNow.AddDays(1))
                    {
                        await _graphClient.Subscriptions[subscription.Id]
                            .PatchAsync(new Subscription
                            {
                                ExpirationDateTime = DateTimeOffset.UtcNow.AddDays(3)
                            });

                        _logger.LogInformation($"Renewed subscription {subscription.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renewing subscriptions");
            }
        }
    }
}
