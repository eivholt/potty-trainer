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
        private readonly IUserData m_userData;

        public CompletedAssignmentsPost(IUserData userData)
        {
            m_userData = userData;
        }

        [FunctionName("CompletedAssignments")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/{userid:int}/completedassignments")] HttpRequest req,
            ILogger log,
            int userid)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic completedAssignment = JsonConvert.DeserializeObject(requestBody);
            

            var user = await m_userData.GetUserWithAssignments(userid);
            user.CompletedAssignments.Add(
                new CompletedAssignment 
                { 
                    Timestamp = DateTime.Now, 
                    Assignment = user.Assignments.First(assignment => assignment.Id.Equals((int)completedAssignment.id)) 
                });

            return new OkObjectResult(user);
        }
    }
}
