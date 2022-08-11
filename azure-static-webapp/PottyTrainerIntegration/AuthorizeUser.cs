using Api;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Api.Table;
using System.Net.Http;
using System.Text.Json;
using Data.TableEntities;
using System.Web;
using Azure;

namespace PottyTrainerIntegration
{
    internal class AuthorizeUser
    {
        private readonly ILogger m_logger;
        private readonly IAuthData m_authData;
        private readonly HttpClient m_httpClient;

        // Todo: Get during init
        private static string m_withingsPottyTrainerClientId = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_CLIENTID");
        private static string m_withingsPottyTrainerClientSecret = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_CLIENTSECRET");
        private static string m_withingsPottyTrainerCallbackUrl = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_CALLBACKURL");

        public AuthorizeUser(ILoggerFactory loggerFactory, IAuthData authData, IHttpClientFactory httpClientFactory)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
            m_authData = authData;
            m_httpClient = httpClientFactory.CreateClient();
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
                string state = queryParameters["state"];
                string code = queryParameters["code"];

                if (string.IsNullOrEmpty(state)) { throw new ArgumentException("Invalid state", nameof(state)); }
                if (string.IsNullOrEmpty(code)) { throw new ArgumentException("Invalid code", nameof(code)); }

                var withingsUrl = "https://wbsapi.withings.net/v2/oauth2";
                var parameters = new Dictionary<string, string>
                {
                    { "action", "requesttoken" },
                    { "client_id", m_withingsPottyTrainerClientId },
                    { "client_secret", m_withingsPottyTrainerClientSecret },
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "redirect_uri", m_withingsPottyTrainerCallbackUrl },
                };
                var encodedContent = new FormUrlEncodedContent(parameters);
                var oauth2Response = await m_httpClient.PostAsync(withingsUrl, encodedContent);

                if (oauth2Response.IsSuccessStatusCode) // 200 - OK from Withings doesn't mean operation was ok..
                {
                    var responseAsJson = JsonSerializer.Deserialize<JsonObject>(await oauth2Response.Content.ReadAsStreamAsync());
                    var status = (int)responseAsJson?["status"];
                    var error = (string)responseAsJson?["error"];

                    if (status > 0)
                    {
                        m_logger.LogError("https://wbsapi.withings.net/v2/oauth2 error:", responseAsJson.ToString());
                    }

                    if (status == 0)
                    {
                        var authResponseBody = responseAsJson?["body"];
                        var systemUserId = (string)authResponseBody?["userid"]!;
                        var accessToken = (string)authResponseBody?["access_token"]!;
                        var refreshToken = (string)authResponseBody?["refresh_token"]!;
                        var expiresIn = (int)authResponseBody?["expires_in"]!;
                        var scope = (string)authResponseBody?["scope"]!;
                        var tokenType = (string)authResponseBody?["token_type"]!;

                        var saveTokenResult = await m_authData.SaveAccessToken(
                            state,
                            systemUserId,
                            UserAuthEntity.WithingsSystemPartitionKeyName,
                            accessToken,
                            refreshToken,
                            DateTime.UtcNow.AddSeconds(expiresIn),
                            scope,
                            tokenType);

                        if (saveTokenResult)
                        {
                            var response = req.CreateResponse(HttpStatusCode.OK);
                            return response;
                        }
                    }
                }
            }
            catch (ArgumentException aex)
            {
                m_logger.LogError($"AccessToken - code or state invalid", aex);
            }
            catch (Exception ex)
            {
                m_logger.LogError($"AccessToken", ex);
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
