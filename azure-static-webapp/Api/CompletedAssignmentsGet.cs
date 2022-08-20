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
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userid}/completedassignmentstoday")] HttpRequest req,
            ILogger log,
            string userId)
        {
            return new OkObjectResult(m_assignmentData.GetCompletedAssignmentsToday(userId));
        }

        [FunctionName("CompletedAssignmentsYesterday")]
        public IActionResult CompletedAssignmentsYesterdayGet(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userid}/completedassignmentsyesterday")] HttpRequest req,
            ILogger log,
            string userId)
        {
            return new OkObjectResult(m_assignmentData.GetCompletedAssignmentsYesterday(userId));
        }

        [FunctionName("AvailableAssignments")]
        public IActionResult AvailableAssignmentsGet(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/availableassignments")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(m_assignmentData.GetAvailableAssignments());
        }
    }
}
