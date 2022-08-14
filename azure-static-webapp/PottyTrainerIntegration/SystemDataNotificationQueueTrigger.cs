using Api;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
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
        private readonly HttpClient m_httpClient;

        public SystemDataNotificationQueueTrigger(ILoggerFactory loggerFactory, IAuthData authData, IUserData userData, IAssignmentData assignmentData, IHttpClientFactory httpClientFactory)
        {
            m_logger = loggerFactory.CreateLogger<SystemDataNotificationQueueTrigger>();
            m_authData = authData;
            m_userData = userData;
            m_assignmentData = assignmentData;
            m_httpClient = httpClientFactory.CreateClient();
        }

        [Function("SystemDataNotificationQueueTrigger")]
        public async Task RunAsync([QueueTrigger("system-data-notification", Connection = "SystemDataNotificationQueueConnection")] string message)
        {
            m_logger.LogInformation($"Queue trigger function processed: {message}");

            //var pipdreamUrl = "https://de8a1a6d2b11e3fdf8f7439d09ed9428.m.pipedream.net";
            //var parameters = new Dictionary<string, string>
            //                    {
            //                        { "message", message }
            //                    };
            //var encodedContent = new FormUrlEncodedContent(parameters);
            //var measureResponse = await m_httpClient.PostAsync(pipdreamUrl, encodedContent);

            try
            {
                var messageAsJson = JsonSerializer.Deserialize<JsonObject>(message);
                var system = (string)messageAsJson?["system"];
                var userId = (string)messageAsJson?["userid"];
                var appli = (string)messageAsJson?["appli"];
                var startDateUnix = (string)messageAsJson?["startdate"];
                var endDateUnix = (string)messageAsJson?["enddate"];

                var userAuth = await m_authData.GetUserAuthBySystemUserId(userId, system);

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
                    var status = (int)responseAsJson?["status"];
                    var error = (string)responseAsJson?["error"];

                    if (status > 0)
                    {
                        m_logger.LogError(withingsMeasureUrl, responseAsJson.ToString());
                    }
                    else
                    {
                        m_logger.LogInformation($"Queue trigger function processed: {responseAsJson}");

                        var xpSum = await m_assignmentData.CompleteAssignment("2766B0C7-CB8A-4568-AE08-9D7BF8D513C8", userAuth.RowKey);
                        var updatedUser = await m_userData.UpdateXp(userAuth.RowKey, xpSum);
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


//var withingsMeasureUrl = "https://wbsapi.withings.net/measure";
//var parameters = new Dictionary<string, string>
//                {
//                    { "action", "getmeas" },
//                    { "meastype", meastype.ToString() },
//                    { "category", category.ToString() },
//                    { "lastupdate", DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds().ToString() }
//                };
//var encodedContent = new FormUrlEncodedContent(parameters);
//m_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAuth.AccessToken);
//var measureResponse = await m_httpClient.PostAsync(withingsMeasureUrl, encodedContent);

//if (measureResponse.IsSuccessStatusCode) // 200 - OK from Withings doesn't mean operation was ok..
//{
//    var responseAsJson = JsonSerializer.Deserialize<JsonObject>(await measureResponse.Content.ReadAsStreamAsync());
//    var status = (int)responseAsJson?["status"];
//    var error = (string)responseAsJson?["error"];

//    if (status > 0)
//    {
//        m_logger.LogError(withingsMeasureUrl, responseAsJson.ToString());
//    }

//    var response = req.CreateResponse(HttpStatusCode.OK);
//    await response.WriteAsJsonAsync(userAuth);
//    return response;
//}
//else
//{
//    throw new InvalidOperationException();
//}