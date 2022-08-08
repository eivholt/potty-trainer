using Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PottyTrainerIntegration
{
    public class CompleteAssignmentForUser
    {
        private readonly ILogger m_logger;
        private readonly IUserData m_userData;
        private readonly IAssignmentData m_assignmentData;

        public CompleteAssignmentForUser(ILoggerFactory loggerFactory, IUserData userData, IAssignmentData assignmentData)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
            m_userData = userData;
            m_assignmentData = assignmentData;
        }

        [Function("CompleteAssignmentForUser")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            m_logger.LogInformation("CompleteAssignmentForUser");

            try
            {
                var requestBodyJsonDom = await JsonSerializer.DeserializeAsync<JsonObject>(req.Body);

                string deviceId = (string)requestBodyJsonDom?["end_device_ids"]!["dev_eui"]!;
                if(deviceId == null)
                {
                    throw new ArgumentNullException(deviceId);
                }

                var user = await m_userData.GetUserFromDeviceId("Dosette", deviceId);
                var xpSum = await m_assignmentData.CompleteAssignment("4390A97B-0323-4787-AC9E-02A5E2B36DEC", user.RowKey);
                var updatedUser = await m_userData.UpdateXp(user.RowKey, xpSum);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(updatedUser);

                return response;
            }
            catch (JsonException jex) 
            {
                m_logger.LogError($"CompleteAssignmentForUser - Error parsing request body", jex);
            }
            catch (ArgumentNullException aex) 
            {
                m_logger.LogError($"CompleteAssignmentForUser - Error parsing request body, end_device_ids.dev_eui not found.", aex);
            }
            catch (Exception ex)
            {
                m_logger.LogError($"CompleteAssignmentForUser - Error getting user by device id", ex);
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
