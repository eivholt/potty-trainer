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
        private readonly IAssignmentData m_userData;

        public CompletedAssignmentsPost(IAssignmentData userData)
        {
            m_userData = userData;
        }

        [FunctionName("CompletedAssignments")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/{userid}/completedassignments")] HttpRequest req,
            ILogger log,
            string userid)
        {
            throw new NotImplementedException();
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic completedAssignmentRequest = JsonConvert.DeserializeObject(requestBody);


            //var user = await m_userData.GetUserWithAssignments(userid);

            //var completedAssignment = new CompletedAssignment
            //{
            //    Timestamp = DateTime.Now,
            //    Assignment = user.Assignments.First(assignment => assignment.Id.Equals((int)completedAssignmentRequest.id))
            //};

            //user.CompletedAssignments.Add(completedAssignment);

            //return new OkObjectResult(completedAssignment);
        }
    }
}
