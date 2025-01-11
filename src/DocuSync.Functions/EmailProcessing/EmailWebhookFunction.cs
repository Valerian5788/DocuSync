using Azure.Storage.Queues;
using DocuSync.Functions.EmailProcessing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace DocuSync.Functions.EmailProcessing
{
    public class EmailWebhookFunction
    {
        private readonly ILogger<EmailWebhookFunction> _logger;
        private readonly QueueClient _queueClient;

        public EmailWebhookFunction(ILogger<EmailWebhookFunction> logger)
        {
            _logger = logger;
            _queueClient = new QueueClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "email-processing");
        }

        [Function("EmailWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Processing email webhook");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var emailMessage = JsonSerializer.Deserialize<EmailMessage>(requestBody);

                // Add to queue
                await _queueClient.SendMessageAsync(JsonSerializer.Serialize(emailMessage));

                var response = req.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email webhook");
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }
        }
    }
}
