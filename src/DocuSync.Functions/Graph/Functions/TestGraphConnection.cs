using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Net;

namespace DocuSync.Functions.Graph.Functions
{
    public class TestGraphConnection
    {
        private readonly ILogger<TestGraphConnection> _logger;
        private readonly GraphServiceClient _graphClient;
        private readonly IConfiguration _configuration;

        public TestGraphConnection(ILogger<TestGraphConnection> logger, GraphServiceClient graphClient, IConfiguration configuration)
        {
            _logger = logger;
            _graphClient = graphClient;
            _configuration = configuration;
        }

        [Function("TestGraphConnection")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var config = _configuration.GetValue<string>("Graph:UserId");
            try
            {
                var messages = await _graphClient.Users[config].Messages
                    .GetAsync(requestConfiguration =>
                    {
                        requestConfiguration.QueryParameters.Top = 1;
                    });

                if (messages?.Value.Count > 0)
                {
                    // Test sending email
                    var message = new Message
                    {
                        Subject = "Test from DocuSync",
                        Body = new ItemBody
                        {
                            ContentType = BodyType.Text,
                            Content = "This is a test email from DocuSync Graph API integration."
                        },
                        ToRecipients = new List<Recipient>
                {
                    new() { EmailAddress = new EmailAddress { Address = "your.test@email.com" } }
                }
                    };

                    await _graphClient.Users["{user-id}"].SendMail.PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                    {
                        Message = message
                    });

                    return req.CreateResponse(HttpStatusCode.OK);
                }

                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing Graph connection");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync(ex.Message);
                return response;
            }
        }
    }
}
