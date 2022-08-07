using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace PottyTrainerIntegration
{
    public class CompleteAssignmentForUser
    {
        private readonly ILogger _logger;

        public CompleteAssignmentForUser(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
        }

        [Function("CompleteAssignmentForUser")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBodyJsonDom = await JsonSerializer.DeserializeAsync<JsonObject>(req.Body);

            string deviceId = (string)requestBodyJsonDom["end_device_ids"]!["dev_eui"]!;


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString(deviceId);

            return response;
        }
    }
}
