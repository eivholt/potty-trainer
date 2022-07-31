using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api
{
    public class CompletedAssignmentsDelete
    {
        private readonly IAssignmentData m_assignmentData;
        private readonly IUserData m_userData;

        public CompletedAssignmentsDelete(IAssignmentData assignmentData, IUserData userData)
        {
            m_assignmentData = assignmentData;
            m_userData = userData;
        }

        [FunctionName("CompletedAssignmentDelete")]
        public async Task<IActionResult> CompletedAssignmentDelete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "users/{userid}/completedassignments/{completedassignmentid}")] HttpRequest req,
            ILogger log,
            string userId,
            string completedassignmentId)
        {
            if(await m_assignmentData.DeleteCompletedAssignment(userId, completedassignmentId))
            {
                var xpSum = await m_assignmentData.CalculateXp(userId);
                var updatedUser = await m_userData.UpdateXp(userId, xpSum);
                return new OkObjectResult(updatedUser);
            }

            return new BadRequestResult();
        }
    }
}