using Api;
using Api.Table;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PottyTrainerIntegration
{
    internal class UserSubscribeSystemPost
    {
        private readonly ILogger m_logger;
        private readonly IAuthData m_authData;
        private readonly HttpClient m_httpClient;

        public UserSubscribeSystemPost(ILoggerFactory loggerFactory, IAuthData authData, IHttpClientFactory httpClientFactory)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
            m_authData = authData;
            m_httpClient = httpClientFactory.CreateClient();
        }

        [Function("UserSubscribeSystemPost")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Users/{userid}/Subscribe/{system}")] HttpRequestData req,
            string userid,
            string system)
        {
            m_logger.LogInformation($"UserSubscribeSystemPost: {userid} - {system}");
            try 
            { 
                var requestBodyJsonDom = await JsonSerializer.DeserializeAsync<JsonObject>(req.Body);
                m_logger.LogInformation($"UserSubscribeSystemPost:", requestBodyJsonDom.ToJsonString());

                var userAuth = await m_authData.GetUserAuth(userid, system);

                var withingsNotifyUrl = "https://wbsapi.withings.net/notify";
                var parameters = new Dictionary<string, string>
                                {
                                    { "action", "subscribe" },
                                    { "callbackurl", "https://pottytrainerintegration20220807232206.azurewebsites.net/api/Users/Notify/withings?code=XZfRuvrXeTe92fEaHGYN8aGQZa3EWCbHYINxMvsULfjlAzFuUzdQiA==" },
                                    { "appli", "1" },
                                    { "signature", "" },
                                    { "nonce", "" },
                                    { "client_id", "" },
                                    { "comment", "" }
                                };
                var encodedContent = new FormUrlEncodedContent(parameters);
                m_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAuth.AccessToken);
                var measureResponse = await m_httpClient.PostAsync(withingsNotifyUrl, encodedContent);

                if (measureResponse.IsSuccessStatusCode) // 200 - OK from Withings doesn't mean operation was ok..
                {
                    var responseAsJson = JsonSerializer.Deserialize<JsonObject>(await measureResponse.Content.ReadAsStreamAsync());
                    var status = (int)responseAsJson?["status"];
                    var error = (string)responseAsJson?["error"];

                    if (status > 0)
                    {
                        m_logger.LogError(withingsNotifyUrl, responseAsJson.ToString());
                    }

                    var response = req.CreateResponse(HttpStatusCode.OK);
                    await response.WriteAsJsonAsync(userAuth);
                    return response;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch
            {
                m_logger.LogError($"UserSubscribeSystemPost");
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }
        }
    }
}

//var meastype = (int)requestBodyJsonDom?["meastype"]!;
//var category = (int)requestBodyJsonDom?["category"]!;

//m_logger.LogInformation($"UserSubscribeSystemPost:", requestBodyJsonDom.ToJsonString());

//var userAuth = await m_authData.GetUserAuth(userid, system);

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