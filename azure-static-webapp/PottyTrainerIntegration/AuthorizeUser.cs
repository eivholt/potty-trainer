using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using PottyTrainerIntegration.OAuth2;
using System.Net;
using System.Web;

namespace PottyTrainerIntegration
{
    internal class AuthorizeUser
    {
        private readonly ILogger m_logger;
        private readonly IOauth2Client m_oauth2Client;

        public AuthorizeUser(ILoggerFactory loggerFactory, IOauth2Client oauth2Client)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
            m_oauth2Client = oauth2Client;
        }

        [Function("AccessToken")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "head", Route = "accesstoken")] HttpRequestData req)
        {
            m_logger.LogInformation("AccessToken");

            if (req.Method.Equals("HEAD")) 
            {
                m_logger.LogInformation("AccessToken - HEAD");
                var response = req.CreateResponse(HttpStatusCode.OK);
                return response;
            }

            try
            {
                var queryParameters = HttpUtility.ParseQueryString(req.Url.Query);
                string state = queryParameters["state"]!;
                string code = queryParameters["code"]!;

                if (string.IsNullOrEmpty(state)) { throw new ArgumentException("Invalid state", nameof(state)); }
                if (string.IsNullOrEmpty(code)) { throw new ArgumentException("Invalid code", nameof(code)); }

                var userAuth = await m_oauth2Client.GetAndStoreAccessToken(code, state);

                var response = req.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (ArgumentException aex)
            {
                m_logger.LogError($"AccessToken - code or state invalid. {aex.Message}", aex);
            }
            catch (Exception ex)
            {
                m_logger.LogError($"AccessToken. {ex.Message}", ex);
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}