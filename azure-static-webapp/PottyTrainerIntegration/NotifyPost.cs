using Api;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PottyTrainerIntegration
{
    internal class NotifyPost
    {
        private readonly ILogger m_logger;

        public NotifyPost(ILoggerFactory loggerFactory, IUserData userData, IAssignmentData assignmentData)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
        }

        [Function("NotifyPost")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Users/Notify/{system}")] HttpRequestData req,
            string system)
        {
            var requestBodyJsonDom = await JsonSerializer.DeserializeAsync<JsonObject>(req.Body);
            m_logger.LogInformation($"NotifyPost: {system}", requestBodyJsonDom.ToJsonString());

            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
