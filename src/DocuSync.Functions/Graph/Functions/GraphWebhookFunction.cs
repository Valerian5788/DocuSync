using DocuSync.Functions.Graph.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Net;
using System.Text.Json;

namespace DocuSync.Functions.Graph.Functions
{
    public class GraphWebhookFunction
    {
        private readonly IGraphMailService _mailService;
        private readonly GraphServiceClient _graphClient;

        public GraphWebhookFunction(IGraphMailService mailService, GraphServiceClient graphClient)
        {
            _mailService = mailService;
            _graphClient = graphClient;
        }

        [Function("GraphWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var notifications = JsonSerializer.Deserialize<ChangeNotificationCollection>(requestBody);

            foreach (var notification in notifications.Value)
            {
                var message = await _graphClient.Users["{user-id}"].Messages[notification.Resource.Split('/').Last()].GetAsync();
                await _mailService.ProcessNewMailAsync(notification.ChangeType, message);
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
