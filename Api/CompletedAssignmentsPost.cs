using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Data;

namespace Api
{
    public class CompletedAssignmentsPost
    {
        private readonly IAssignmentData m_assignmentData;

        public CompletedAssignmentsPost(IAssignmentData assignmentData)
        {
            m_assignmentData = assignmentData;
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
                await m_assignmentData.CompleteAssignment(assignmentid, userid, DateTime.UtcNow);
            }
            catch
            {
                return new BadRequestResult();
            }

            return new OkResult();
        }
    }
}
