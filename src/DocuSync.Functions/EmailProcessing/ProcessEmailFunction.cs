using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using DocuSync.Functions.EmailProcessing.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DocuSync.Functions.EmailProcessing
{
    public class ProcessEmailFunction
    {
        private readonly ILogger<ProcessEmailFunction> _logger;
        private readonly EmailProcessor _processor;

        public ProcessEmailFunction(ILogger<ProcessEmailFunction> logger, EmailProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        [Function("ProcessEmail")]
        public async Task RunAsync([QueueTrigger("email-processing")] string message)
        {
            try
            {
                var emailMessage = JsonSerializer.Deserialize<EmailMessage>(message);
                await _processor.ProcessEmailAsync(emailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email from queue");
                throw; // Retry queue processing
            }
        }
    }
}
