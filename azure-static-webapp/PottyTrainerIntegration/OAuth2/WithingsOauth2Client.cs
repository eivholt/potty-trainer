using Api;
using Data;
using Data.TableEntities;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PottyTrainerIntegration.OAuth2
{
    public class WithingsOauth2Client : IOauth2Client
    {
        private readonly ILogger m_logger;
        private readonly IAuthData m_authData;
        private readonly HttpClient m_httpClient;
        private readonly string m_withingsOauth2Url = "https://wbsapi.withings.net/v2/oauth2";
        private const int m_withingsInvalidAccessTokenStatusCode = 401;

        private static string m_withingsPottyTrainerClientId = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_CLIENTID")!;
        private static string m_withingsPottyTrainerClientSecret = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_CLIENTSECRET")!;
        private static string m_withingsPottyTrainerCallbackUrl = Environment.GetEnvironmentVariable("WITHINGS_POTTYTRAINER_CALLBACKURL")!;

        public static int InvalidAccessToken { get => m_withingsInvalidAccessTokenStatusCode; }

        public WithingsOauth2Client(ILoggerFactory loggerFactory, IAuthData authData, IHttpClientFactory httpClientFactory)
        {
            m_logger = loggerFactory.CreateLogger<WithingsOauth2Client>();
            m_authData = authData;
            m_httpClient = httpClientFactory.CreateClient();
        }

        public async Task<UserAuth> GetAndStoreAccessToken(string code, string state)
        {
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
            var oauth2Response = await m_httpClient.PostAsync(m_withingsOauth2Url, encodedContent);

            if (oauth2Response.IsSuccessStatusCode) // 200 - OK from Withings doesn't mean operation was ok..
            {
                var responseAsJson = JsonSerializer.Deserialize<JsonObject>(await oauth2Response.Content.ReadAsStreamAsync());
                var status = (int)responseAsJson?["status"]!;
                var error = (string)responseAsJson?["error"]!;

                if (status > 0)
                {
                    m_logger.LogError("https://wbsapi.withings.net/v2/oauth2 error:", responseAsJson!.ToString());
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
                        UserAuthEntity.WithingsSystemPartitionKeyName,
                        systemUserId,
                        accessToken,
                        refreshToken,
                        DateTime.UtcNow.AddSeconds(expiresIn),
                        scope,
                        tokenType);
                    return saveTokenResult;
                }
            }

            throw new Exception("GetAndStoreAccessToken failed.");
        }

        public async Task<UserAuth> RefreshAccessTokenAndStore(string oldRefreshToken)
        {
            var parameters = new Dictionary<string, string>
            {
                { "action", "requesttoken" },
                { "client_id", m_withingsPottyTrainerClientId },
                { "client_secret", m_withingsPottyTrainerClientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", oldRefreshToken },
            };
            var encodedContent = new FormUrlEncodedContent(parameters);
            var oauth2Response = await m_httpClient.PostAsync(m_withingsOauth2Url, encodedContent);

            if (oauth2Response.IsSuccessStatusCode) // 200 - OK from Withings doesn't mean operation was ok..
            {
                var responseAsJson = JsonSerializer.Deserialize<JsonObject>(await oauth2Response.Content.ReadAsStreamAsync());
                var status = (int)responseAsJson?["status"]!;
                var error = (string)responseAsJson?["error"]!;

                if (status > 0)
                {
                    m_logger.LogError("withings refresh token error:", responseAsJson.ToString());
                }

                var authResponseBody = responseAsJson?["body"];
                var systemUserId = (int)authResponseBody?["userid"]!;
                var accessToken = (string)authResponseBody?["access_token"]!;
                var refreshToken = (string)authResponseBody?["refresh_token"]!;
                var expiresIn = (int)authResponseBody?["expires_in"]!;
                var scope = (string)authResponseBody?["scope"]!;
                var tokenType = (string)authResponseBody?["token_type"]!;

                var refreshUserAuth = await m_authData.RefreshAccessToken(
                    systemUserId.ToString(),
                    UserAuthEntity.WithingsSystemPartitionKeyName,
                    accessToken,
                    refreshToken,
                    DateTime.UtcNow.AddSeconds(expiresIn),
                    scope,
                    tokenType);

                return refreshUserAuth;
            }
            else
            {
                throw new InvalidOperationException("RefreshAccessToken: withings failed.");
            }

        }
    }
}