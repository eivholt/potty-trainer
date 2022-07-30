using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class CompletedAssignmentsGet
    {
        private readonly IAssignmentData m_assignmentData;

        public CompletedAssignmentsGet(IAssignmentData assignmentData)
        {
            m_assignmentData = assignmentData;
        }

        [FunctionName("CompletedAssignmentsToday")]
        public IActionResult CompletedAssignmentsTodayGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userid}/completedassignmentstoday")] HttpRequest req,
            ILogger log,
            string userId)
        {
            return new OkObjectResult(m_assignmentData.GetCompletedAssignmentsToday(userId));
        }
    }
}
