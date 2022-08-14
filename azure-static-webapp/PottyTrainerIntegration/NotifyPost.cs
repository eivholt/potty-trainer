using Api;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;

namespace PottyTrainerIntegration
{
    internal class NotifyPost
    {
        private const string c_incomingQueueName = "system-data-notification";
        private readonly ILogger m_logger;
        private readonly QueueClient m_queueClient;

        public NotifyPost(ILoggerFactory loggerFactory, IUserData userData, IAssignmentData assignmentData)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
            m_queueClient = CreateQueueClient(c_incomingQueueName);
        }

        [Function("NotifyPost")]
        [QueueOutput(c_incomingQueueName)]
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

            var multipartFormDataParser = await MultipartFormDataParser.ParseAsync(req.Body).ConfigureAwait(false);
            m_logger.LogInformation($"NotifyPost: {system}", multipartFormDataParser.Parameters.ToString());

            var userid = multipartFormDataParser.GetParameterValue("userid");
            var appli = multipartFormDataParser.GetParameterValue("appli");
            var startdate = multipartFormDataParser.GetParameterValue("startdate");
            var enddate = multipartFormDataParser.GetParameterValue("enddate");

            dynamic withingsNotification = new
            {
                system = system,
                userid = multipartFormDataParser.GetParameterValue("userid"),
                appli = multipartFormDataParser.GetParameterValue("appli"),
                startdate = multipartFormDataParser.GetParameterValue("startdate"),
                enddate = multipartFormDataParser.GetParameterValue("enddate")
            };

            var notificationMessageAsJson = JsonSerializer.Serialize(withingsNotification);

            await InsertMessageAsync(m_queueClient, withingsNotification);

            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public static QueueClient CreateQueueClient(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            return new QueueClient(connectionString, queueName);
        }

        public async Task InsertMessageAsync(QueueClient queueClient, string message)
        {
            // Create the queue if it doesn't already exist
            await queueClient.CreateAsync();
            await queueClient.SendMessageAsync(message);

            m_logger.LogInformation($"NotifyPost: Sent message to queue: {queueClient.Name}", message);
        }
    }
}
