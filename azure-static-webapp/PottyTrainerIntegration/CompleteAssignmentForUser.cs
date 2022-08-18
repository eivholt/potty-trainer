using Api;
using Data;
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
        private const string m_dosetteSystemName = "dosette";
        private const string m_gotmailSystemName = "gotmail";
        private const string m_dosetteAssignmentId = "4390A97B-0323-4787-AC9E-02A5E2B36DEC";
        private const string m_gotmailAssignmentId = "4187A11D-ABCA-4AEA-AF1B-C8E23CB28D96";
        private const string m_gotmailAndHousePlantsUserKey = "BAA25D7F-4462-482F-B919-83F938FC72D3"; // Eivind

        public CompleteAssignmentForUser(ILoggerFactory loggerFactory, IUserData userData, IAssignmentData assignmentData)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
            m_userData = userData;
            m_assignmentData = assignmentData;
        }

        [Function("CompleteAssignmentForUser")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/completedassignments/{system}")] HttpRequestData req,
            string system)
        {
            m_logger.LogInformation("CompleteAssignmentForUser");

            if (string.IsNullOrEmpty(system))
            {
                throw new ArgumentNullException(nameof(system));
            }

            m_logger.LogInformation($"system: {system}.");

            try
            {
                var requestBodyJsonDom = await JsonSerializer.DeserializeAsync<JsonObject>(req.Body);

                if (system.Equals(m_dosetteSystemName))
                {
                    string deviceId = (string)requestBodyJsonDom?["end_device_ids"]!["dev_eui"]!;
                    if (string.IsNullOrEmpty(deviceId))
                    {
                        throw new ArgumentNullException(nameof(deviceId));
                    }

                    var user = await m_userData.GetUserFromDeviceId(system, deviceId);
                    User updatedUser = await CreateCompletedAssignmentAndUpdateXp(user.RowKey, m_dosetteAssignmentId);

                    var response = req.CreateResponse(HttpStatusCode.OK);
                    await response.WriteAsJsonAsync(updatedUser);

                    return response;
                }
                else if (system.Equals(m_gotmailSystemName))
                {
                    var availableAssignmentsGotMailToday = m_assignmentData.AvailableAssignmentTypeTodayExists(m_gotmailAssignmentId);

                    // if aa exists, complete
                    if (await availableAssignmentsGotMailToday.CountAsync() > 0)
                    {
                        var firstAvailableAssignmentOfType = await availableAssignmentsGotMailToday.FirstAsync();
                        var xpSum = await m_assignmentData.CompleteAvailableAssignment(firstAvailableAssignmentOfType.RowKey, m_gotmailAndHousePlantsUserKey);
                        var updatedUser = await m_userData.UpdateXp(m_gotmailAndHousePlantsUserKey, xpSum);
                    }

                    // if aa not exists, create aa
                    else 
                    {
                        await m_assignmentData.AddAvailableAssignment(m_gotmailAssignmentId, system);
                    }

                    return req.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (JsonException jex) 
            {
                m_logger.LogError($"CompleteAssignmentForUser - Error parsing request body.", jex);
            }
            catch (ArgumentNullException aex) 
            {
                m_logger.LogError($"CompleteAssignmentForUser - Error parsing request.", aex);
            }
            catch (Exception ex)
            {
                m_logger.LogError($"CompleteAssignmentForUser - Error getting user.", ex);
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        private async Task<User> CreateCompletedAssignmentAndUpdateXp(string userKey, string assignmentId)
        {
            var xpSum = await m_assignmentData.CompleteAssignment(assignmentId, userKey);
            var updatedUser = await m_userData.UpdateXp(userKey, xpSum);
            return updatedUser;
        }
    }
}
