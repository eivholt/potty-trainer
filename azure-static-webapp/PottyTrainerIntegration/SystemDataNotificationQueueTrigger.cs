using Api;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PottyTrainerIntegration
{
    public class SystemDataNotificationQueueTrigger
    {
        private readonly ILogger m_logger;
        private readonly IAuthData m_authData;
        private readonly HttpClient m_httpClient;

        public SystemDataNotificationQueueTrigger(ILoggerFactory loggerFactory, IAuthData authData, IHttpClientFactory httpClientFactory)
        {
            m_logger = loggerFactory.CreateLogger<SystemDataNotificationQueueTrigger>();
            m_authData = authData;
            m_httpClient = httpClientFactory.CreateClient();
        }

        [Function("SystemDataNotificationQueueTrigger")]
        public async Task RunAsync([QueueTrigger("system-data-notification", Connection = "SystemDataNotificationQueueConnection")] string message)
        {
            m_logger.LogInformation($"Queue trigger function processed: {message}");

            var pipdreamUrl = "https://de8a1a6d2b11e3fdf8f7439d09ed9428.m.pipedream.net";
            var parameters = new Dictionary<string, string>
                                {
                                    { "message", message }
                                };
            var encodedContent = new FormUrlEncodedContent(parameters);
            var measureResponse = await m_httpClient.PostAsync(pipdreamUrl, encodedContent);
        }
    }
}