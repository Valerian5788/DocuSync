using Azure.Identity;
using DocuSync.Functions.EmailProcessing.Models;
using DocuSync.Functions.EmailProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace DocuSync.Functions.Graph.Services
{
    public class GraphMailService : IGraphMailService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly IEmailProcessor _emailProcessor;
        private readonly ILogger<GraphMailService> _logger;
        private readonly string _notificationUrl;

        public GraphMailService(
            IConfiguration configuration,
            IEmailProcessor emailProcessor,
            ILogger<GraphMailService> logger)
        {
            // Setup Graph client with client credentials
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var clientSecretCredential = new ClientSecretCredential(
                configuration["Graph:TenantId"],
                configuration["Graph:ClientId"],
                configuration["Graph:ClientSecret"]);

            _graphClient = new GraphServiceClient(clientSecretCredential, scopes);
            _emailProcessor = emailProcessor;
            _logger = logger;
            _notificationUrl = configuration["Graph:NotificationUrl"];
        }

        public async Task ProcessNewMailAsync(ChangeType? changeType, Message message)
        {
            try
            {
                if (changeType != ChangeType.Created)
                    return;

                // Convert to our email model
                var emailMessage = new EmailMessage
                {
                    From = message.From.EmailAddress.Address,
                    Subject = message.Subject,
                    Attachments = await GetAttachmentsAsync(message.Id)
                };

                await _emailProcessor.ProcessEmailAsync(emailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing mail {MessageId}", message.Id);
            }
        }

        private async Task<List<EmailAttachment>> GetAttachmentsAsync(string messageId)
        {
            var attachments = new List<EmailAttachment>();
            var message = await _graphClient.Users["{user-id}"].Messages[messageId].Attachments.GetAsync();

            foreach (var attachment in message.Value)
            {
                if (attachment is FileAttachment fileAttachment)
                {
                    attachments.Add(new EmailAttachment
                    {
                        FileName = fileAttachment.Name,
                        ContentType = fileAttachment.ContentType,
                        Content = fileAttachment.ContentBytes
                    });
                }
            }

            return attachments;
        }

        public async Task SetupSubscriptionAsync()
        {
            var subscription = new Subscription
            {
                ChangeType = "created",
                NotificationUrl = _notificationUrl,
                Resource = "/users/{user-id}/messages",
                ExpirationDateTime = DateTimeOffset.UtcNow.AddDays(3),
                ClientState = Guid.NewGuid().ToString()
            };

            await _graphClient.Subscriptions.PostAsync(subscription);
        }
    }
}
