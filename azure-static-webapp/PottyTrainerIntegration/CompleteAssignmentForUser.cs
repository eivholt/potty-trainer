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
        private const string m_houseplantsSystemName = "houseplants";
        private const string m_dosetteAssignmentId = "4390A97B-0323-4787-AC9E-02A5E2B36DEC";
        private const string m_gotmailAssignmentId = "4187A11D-ABCA-4AEA-AF1B-C8E23CB28D96";
        private const string m_houseplants1AssignmentId = "E54ABEA8-F97F-48FD-884B-B8FBB38323FC";
        private const string m_houseplants2AssignmentId = "4DFD8832-7C27-45C2-A370-FC9A3656F926";
        private const string m_houseplants3AssignmentId = "BC72D11B-E34C-494A-B882-B703FB410220";
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
                    var deviceId = (string)requestBodyJsonDom?["end_device_ids"]!["dev_eui"]!;
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
                    await CreateOrCompleteAvailableAssignment(system, m_gotmailAssignmentId);
                    return req.CreateResponse(HttpStatusCode.OK);
                }
                else if (system.Equals(m_houseplantsSystemName))
                {
                    var capacitance1 = (int)requestBodyJsonDom?["uplink_message"]!["decoded_payload"]!["capacitance1"]!;
                    var capacitance2 = (int)requestBodyJsonDom?["uplink_message"]!["decoded_payload"]!["capacitance2"]!;
                    var capacitance3 = (int)requestBodyJsonDom?["uplink_message"]!["decoded_payload"]!["capacitance3"]!;
                    
                    if (capacitance1 <= 300)
                    {
                        await CreateAvailableAssignmentIfNotExists(system, m_houseplants1AssignmentId);
                    }
                    else
                    {
                        await CompleteAvailableAssignmentIfExists(m_houseplants1AssignmentId);
                    }

                    if (capacitance2 <= 300)
                    {
                        await CreateAvailableAssignmentIfNotExists(system, m_houseplants2AssignmentId);
                    }
                    else
                    {
                        await CompleteAvailableAssignmentIfExists(m_houseplants2AssignmentId);
                    }

                    if (capacitance3 <= 300)
                    {
                        await CreateAvailableAssignmentIfNotExists(system, m_houseplants3AssignmentId);
                    }
                    else
                    {
                        await CompleteAvailableAssignmentIfExists(m_houseplants3AssignmentId);
                    }
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

        private async Task CreateOrCompleteAvailableAssignment(string system, string assignmentId)
        {
            var availableAssignmentsToday = m_assignmentData.AvailableAssignmentTypeTodayExists(assignmentId);

            // if aa exists, complete
            if (await availableAssignmentsToday.CountAsync() > 0)
            {
                m_logger.LogInformation("Complete available assignment for user.");
                var firstAvailableAssignmentOfType = await availableAssignmentsToday.FirstAsync();
                var xpSum = await m_assignmentData.CompleteAvailableAssignment(firstAvailableAssignmentOfType.RowKey, m_gotmailAndHousePlantsUserKey);
                var updatedUser = await m_userData.UpdateXp(m_gotmailAndHousePlantsUserKey, xpSum);
                m_logger.LogInformation("Complete available assignment for user done.");
            }

            // if aa not exists, create aa
            else
            {
                await AddNewAvailableAssignment(system, assignmentId);
            }
        }

        private async Task CreateAvailableAssignmentIfNotExists(string system, string assignmentId)
        {
            var availableAssignmentsToday = m_assignmentData.AvailableAssignmentTypeTodayExists(assignmentId);

            // if aa exists, complete
            if (await availableAssignmentsToday.CountAsync() == 0)
            {
                await AddNewAvailableAssignment(system, assignmentId);
            }
        }

        private async Task AddNewAvailableAssignment(string system, string assignmentId)
        {
            m_logger.LogInformation($"Create available assignment, system: {system}, assignmentId: {assignmentId}.");
            var availableAssignment = await m_assignmentData.AddAvailableAssignment(assignmentId, system);
            m_logger.LogInformation($"Create available assignment done, {availableAssignment.RowKey}.");
        }
        private async Task CompleteAvailableAssignmentIfExists(string assignmentId)
        {
            var availableAssignmentsToday = m_assignmentData.AvailableAssignmentTypeTodayExists(assignmentId);

            // if aa exists, complete
            if (await availableAssignmentsToday.CountAsync() > 0)
            {
                m_logger.LogInformation("Complete available assignment for user.");
                var firstAvailableAssignmentOfType = await availableAssignmentsToday.FirstAsync();
                var xpSum = await m_assignmentData.CompleteAvailableAssignment(firstAvailableAssignmentOfType.RowKey, m_gotmailAndHousePlantsUserKey);
                var updatedUser = await m_userData.UpdateXp(m_gotmailAndHousePlantsUserKey, xpSum);
                m_logger.LogInformation("Complete available assignment for user done.");
            }
        }

        private async Task<User> CreateCompletedAssignmentAndUpdateXp(string userKey, string assignmentId)
        {
            m_logger.LogInformation($"Updating XP for user: {userKey}.");
            var xpSum = await m_assignmentData.CompleteAssignment(assignmentId, userKey);
            var updatedUser = await m_userData.UpdateXp(userKey, xpSum);
            m_logger.LogInformation($"Done updating XP for user: {updatedUser.Name}, xp: {updatedUser.XP}.");
            return updatedUser;
        }
    }
}
