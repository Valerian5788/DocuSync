using DocuSync.Functions.EmailProcessing.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DocuSync.Application.Services.Interfaces;
using DocuSync.Domain.Entities;
using DocuSync.Application.Services;
using DocuSync.Application.Common.Results;


namespace DocuSync.Functions.EmailProcessing
{
    public interface IEmailProcessor
    {
        Task ProcessEmailAsync(EmailMessage message);
    }

    public class EmailProcessor : IEmailProcessor
    {
        private readonly ILogger<EmailProcessor> _logger;
        private readonly IClientService _clientService;
        private readonly IDocumentService _documentService;
        private readonly SmtpClient _smtpClient;
        private readonly IRequirementService _requirementService;

        public EmailProcessor(
            ILogger<EmailProcessor> logger,
            IClientService clientService,
            IDocumentService documentService,
            IConfiguration configuration,
            IRequirementService requirementService)
        {
            _logger = logger;
            _clientService = clientService;
            _documentService = documentService;
            _requirementService = requirementService;

            // Configure SMTP
            _smtpClient = new SmtpClient
            {
                Host = configuration["Email:SmtpServer"],
                Port = int.Parse(configuration["Email:SmtpPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    configuration["Email:Username"],
                    configuration["Email:Password"])
            };
        }

        public async Task ProcessEmailAsync(EmailMessage message)
        {
            try
            {
                // 1. Validate and get client
                var clientResult = await _clientService.GetByUserEmailAsync(message.From);
                if (!clientResult.IsSuccess)
                {
                    _logger.LogWarning($"Unknown sender: {message.From}");
                    return;
                }
                var client = clientResult.Data;

                // 2. Process each attachment
                foreach (var attachment in message.Attachments)
                {
                    try
                    {
                        // Create memory stream from attachment
                        using var stream = new MemoryStream(attachment.Content);

                        // Get active requirements for client
                        var requirements = await _requirementService.GetActiveForClientAsync(client.Id);

                        // For MVP: Use first active requirement
                        // TODO: Implement smart requirement matching
                        var requirement = requirements.FirstOrDefault();
                        if (requirement == null)
                        {
                            _logger.LogWarning($"No active requirements for client {client.Name}");
                            continue;
                        }

                        // Upload and forward
                        await ProcessDocumentAsync(client, requirement, stream, attachment);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing attachment {attachment.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email");
                throw;
            }
        }

        private async Task ProcessDocumentAsync(
        Client client,
        Requirement requirement,
        Stream content,
        EmailAttachment attachment)
        {
            // Upload to document service
            var uploadResult = await _documentService.UploadForRequirementAsync(
                requirement.Id,
                content,
                attachment.FileName);

            if (uploadResult.Success)
            {
                // Forward to Horus
                await ForwardToHorusAsync(
                    client.HorusEmail,
                    $"Document for {requirement.DocumentType.Name}",
                    attachment);
            }
        }

        private async Task ForwardToHorusAsync(
            string horusEmail,
            string subject,
            EmailAttachment attachment)
        {
            var mail = new MailMessage
            {
                From = new MailAddress("docusync@yourdomain.com", "DocuSync"),
                Subject = $"FWD: {subject}",
                Body = "Automated document forwarding by DocuSync"
            };

            mail.To.Add(horusEmail);

            // Attach the document
            mail.Attachments.Add(new Attachment(
                new MemoryStream(attachment.Content),
                attachment.FileName,
                attachment.ContentType));

            await _smtpClient.SendMailAsync(mail);
        }
    }
}
