using Api;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace PottyTrainerIntegration
{
    internal class NotifyPost
    {
        private const string c_incomingQueueName = "system-data-notification";
        private readonly ILogger m_logger;
        private readonly QueueClient m_queueClient;

        public NotifyPost(ILoggerFactory loggerFactory, IUserData userData, IAssignmentData assignmentData)
        {
            m_logger = loggerFactory.CreateLogger<NotifyPost>();
            m_queueClient = CreateQueueClient(c_incomingQueueName);
        }

        [Function("NotifyPost")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", "head", Route = "Users/Notify/{system}")] HttpRequestData req,
            string system)
        {
            m_logger.LogInformation($"NotifyPost: {system}", req.Headers.ToString());

            if (req.Method.Equals("HEAD"))
            {
                m_logger.LogInformation("NotifyPost - HEAD");
                var headResponse = req.CreateResponse(HttpStatusCode.OK);
                return headResponse;
            }

            var requestParameters = HttpUtility.ParseQueryString(await req.ReadAsStringAsync());
            m_logger.LogInformation($"NotifyPost: {system}", requestParameters);

            var userid = requestParameters["userid"];
            var appli = requestParameters["appli"];
            var startdate = requestParameters["startdate"];
            var enddate = requestParameters["enddate"];

            dynamic withingsNotification = new
            {
                system = system,
                userid = userid,
                appli = appli,
                startdate = startdate,
                enddate = enddate
            };

            var notificationMessageAsJson = JsonSerializer.Serialize(withingsNotification);

            await InsertMessageAsync(m_queueClient, notificationMessageAsJson);

            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public static QueueClient CreateQueueClient(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = Environment.GetEnvironmentVariable("SystemDataNotificationQueueConnection", EnvironmentVariableTarget.Process);

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            return new QueueClient(connectionString, queueName);
        }

        public async Task InsertMessageAsync(QueueClient queueClient, string message)
        {
            // Create the queue if it doesn't already exist
            //await queueClient.CreateAsync();
            await queueClient.SendMessageAsync(Base64Encode(message));

            m_logger.LogInformation($"NotifyPost: Sent message to queue: {queueClient.Name}", message);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
