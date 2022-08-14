using Api;
using Api.Table;
using Azure;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace PottyTrainerIntegration
{
    internal class UserSubscribeSystemPost
    {
        private readonly ILogger m_logger;
        private readonly IAuthData m_authData;
        private readonly HttpClient m_httpClient;

        private static string m_withingsPottyTrainerClientId = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_CLIENTID");
        private static string m_withingsPottyTrainerClientSecret = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_CLIENTSECRET");
        private static string m_withingsPottyTrainerNotifyUrl = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_NOTIFYURL");

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
                var userAuth = await m_authData.GetUserAuth(userid, system);

                var action = "subscribe";
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var signature = GetWithingsHash(action + "," + m_withingsPottyTrainerClientId + "," + timestamp, m_withingsPottyTrainerClientSecret);
                var nonce = await GetWithingsNonce(m_httpClient, m_withingsPottyTrainerClientId, timestamp, m_withingsPottyTrainerClientSecret);
                var withingsNotifyUrl = "https://wbsapi.withings.net/notify";
                var parameters = new Dictionary<string, string>
                                {
                                    { "action", action },
                                    { "callbackurl", m_withingsPottyTrainerNotifyUrl },
                                    { "appli", "1" },
                                    { "signature", signature },
                                    { "nonce", nonce },
                                    { "client_id", m_withingsPottyTrainerClientId },
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

        private async Task<string> GetWithingsNonce(HttpClient httpClient, string clientId, string timestamp, string key)
        {
            var withingsSignatureUrl = "https://wbsapi.withings.net/v2/signature";

            var action = "getnonce";
            var signature = GetWithingsHash(action + "," + clientId + "," + timestamp, key);

            var parameters = new Dictionary<string, string>
                                {
                                    { "action", action },
                                    { "client_id", clientId },
                                    { "timestamp", timestamp },
                                    { "signature", signature }
                                };
            var encodedContent = new FormUrlEncodedContent(parameters);
            var signatureResponse = await m_httpClient.PostAsync(withingsSignatureUrl, encodedContent);
            signatureResponse.EnsureSuccessStatusCode();
            var signatureJsonResponse = JsonSerializer.Deserialize<JsonObject>(await signatureResponse.Content.ReadAsStreamAsync());
            EnsureWithingsSuccessStatusCode(signatureJsonResponse);
            var signatureBody = signatureJsonResponse?["body"]!;

            return (string)signatureBody?["nonce"]!;
        }

        private static string GetWithingsHash(string text, string key)
        {
            var asciiEncoding = new ASCIIEncoding();
            byte[] bytes = asciiEncoding.GetBytes(text);
            byte[] hash;
            using (HMACSHA256 hmacshA256 = new HMACSHA256(asciiEncoding.GetBytes(key)))
                hash = hmacshA256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private static void EnsureWithingsSuccessStatusCode(JsonObject? jsonResponse)
        {
            var status = (int)jsonResponse?["status"]!;
            var error = (string)jsonResponse?["error"]!;

            if (status > 0)
                throw new RequestFailedException(error);
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