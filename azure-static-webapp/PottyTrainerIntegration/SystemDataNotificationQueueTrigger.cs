using Api;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PottyTrainerIntegration.OAuth2;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PottyTrainerIntegration
{
    public class SystemDataNotificationQueueTrigger
    {
        private readonly ILogger m_logger;
        private readonly IAuthData m_authData;
        private readonly IUserData m_userData;
        private readonly IAssignmentData m_assignmentData;
        private readonly IOauth2Client m_oauth2Client;
        private readonly HttpClient m_httpClient;

        public SystemDataNotificationQueueTrigger(
            ILoggerFactory loggerFactory, 
            IAuthData authData, 
            IUserData userData, 
            IAssignmentData assignmentData, 
            IOauth2Client oauth2Client, 
            IHttpClientFactory httpClientFactory)
        {
            m_logger = loggerFactory.CreateLogger<SystemDataNotificationQueueTrigger>();
            m_authData = authData;
            m_userData = userData;
            m_assignmentData = assignmentData;
            m_oauth2Client = oauth2Client;
            m_httpClient = httpClientFactory.CreateClient();
        }

        [Function("SystemDataNotificationQueueTrigger")]
        public async Task RunAsync([QueueTrigger("system-data-notification", Connection = "SystemDataNotificationQueueConnection")] string message)
        {
            m_logger.LogInformation($"Queue trigger function processed: {message}");
            
            try
            {
                var messageAsJson = JsonSerializer.Deserialize<JsonObject>(message);
                var system = (string)messageAsJson?["system"]!;
                var userId = (string)messageAsJson?["userid"]!;
                var appli = (string)messageAsJson?["appli"]!;
                var startDateUnix = (string)messageAsJson?["startdate"]!;
                var endDateUnix = (string)messageAsJson?["enddate"]!;

                var userAuth = await m_authData.GetUserAuthBySystemUserId(userId, system);

                if (userAuth.Expires < DateTime.UtcNow)
                {
                    userAuth = await m_oauth2Client.RefreshAccessTokenAndStore(userAuth.RefreshToken);
                }

                var withingsMeasureUrl = "https://wbsapi.withings.net/measure";
                var measureParameters = new Dictionary<string, string>
                                {
                                    { "action", "getmeas" },
                                    { "meastype", "1" },
                                    { "category", "1" },
                                    { "startdate", startDateUnix },
                                    { "enddate", endDateUnix }
                                };
                var encodedContent = new FormUrlEncodedContent(measureParameters);
                m_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAuth.AccessToken);
                var measureResponse = await m_httpClient.PostAsync(withingsMeasureUrl, encodedContent);

                if (measureResponse.IsSuccessStatusCode) // 200 - OK from Withings doesn't mean operation was ok..
                {
                    var responseAsJson = JsonSerializer.Deserialize<JsonObject>(await measureResponse.Content.ReadAsStreamAsync());
                    var status = (int)responseAsJson?["status"]!;
                    var error = (string)responseAsJson?["error"]!;
                    m_logger.LogInformation($"Queue trigger function processed: {status}, {error} - {responseAsJson}");

                    if (status == 401)
                    {
                        m_logger.LogError(withingsMeasureUrl, responseAsJson!.ToString());
                    }
                    if (status > 0)
                    {
                        m_logger.LogError($"SystemDataNotificationQueueTrigger: {status}, {error}", responseAsJson!.ToString());
                    }
                    else
                    {
                        m_logger.LogInformation($"Queue trigger function processed: {responseAsJson}");


                        //BP: "59DD55D3-9F48-4B56-A905-BB433FF5441F"
                        try
                        {
                            var xpSum = await m_assignmentData.CompleteAssignment("2766B0C7-CB8A-4568-AE08-9D7BF8D513C8", userAuth.RowKey);
                            var updatedUser = await m_userData.UpdateXp(userAuth.RowKey, xpSum);
                        }
                        catch (Exception ex)
                        {
                            m_logger.LogError($"Queue trigger function failed, CompleteAssignment: {userAuth.RowKey}", ex);
                        }
                    }
                }
            }
            catch
            {
                m_logger.LogError($"Queue trigger function failed: {message}");
            }
        }
    }
}