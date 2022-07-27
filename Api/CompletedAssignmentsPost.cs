using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api
{
    public class CompletedAssignmentsPost
    {
        private readonly IAssignmentData m_assignmentData;
        private readonly IUserData m_userData;

        public CompletedAssignmentsPost(IAssignmentData assignmentData, IUserData userData)
        {
            m_assignmentData = assignmentData;
            m_userData = userData;
        }

        [FunctionName("CompletedAssignments")]
        public async Task<IActionResult> CompleteUserAssignmentPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/{userid}/completedassignment/{assignmentid}")] HttpRequest req,
            ILogger log,
            string userid,
            string assignmentid)
        {
            try
            {
                var xpSum = await m_assignmentData.CompleteAssignment(assignmentid, userid, DateTime.UtcNow);
                await m_userData.UpdateXp(userid, xpSum);
            }
            catch
            {
                return new BadRequestResult();
            }

            return new OkResult();
        }
    }
}
