using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace PottyTrainerIntegration
{
    internal class FaviconGet
    {
        private readonly ILogger m_logger;

        public FaviconGet(ILoggerFactory loggerFactory)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
        }

        [Function("Favicon")]
        public Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "favicon.ico")] HttpRequestData req)
        {
            m_logger.LogInformation("Favicon");

            var response = req.CreateResponse(HttpStatusCode.OK);

            return Task.FromResult(response);
        }
    }
}
