using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PottyTrainerIntegration
{
    public class CompleteAssignmentForUser
    {
        private readonly ILogger m_logger;

        public CompleteAssignmentForUser(ILoggerFactory loggerFactory)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
        }

        [Function("CompleteAssignmentForUser")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            m_logger.LogInformation("CompleteAssignmentForUser");

            try
            {
                var requestBodyJsonDom = await JsonSerializer.DeserializeAsync<JsonObject>(req.Body);

                string deviceId = (string)requestBodyJsonDom["end_device_ids"]!["dev_eui"]!;


                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                response.WriteString(deviceId);

                return response;
            }
            catch (JsonException jex) 
            {
                m_logger.LogError($"CompleteAssignmentForUser - Error parsing request body", jex);
            }
            catch (ArgumentNullException aex) 
            {
                m_logger.LogError($"CompleteAssignmentForUser - Error parsing request body, end_device_ids.dev_eui not found.", aex);
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
