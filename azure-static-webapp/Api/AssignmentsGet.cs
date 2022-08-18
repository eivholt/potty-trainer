using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Data;
using System.Linq;

namespace Api
{
    public class AssignmentsGet
    {
        private readonly IAssignmentsForUser m_assignmentsForUserData;
        private readonly IUserData m_userData;

        public AssignmentsGet(IAssignmentsForUser assignmentsForUserData, IUserData userData)
        {
            m_assignmentsForUserData = assignmentsForUserData;
            m_userData = userData;
        }

        [FunctionName("UserWithAssignmentsGet")]
        public async Task<IActionResult> GetUserWithAssignments(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userid}/assignments")] HttpRequest req,
            ILogger log,
            string userId)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return new BadRequestResult();
            }
            else
            {
                User user;
                try { user = await m_userData.GetUser(userGuid.ToString()); }
                catch (Azure.RequestFailedException)
                {
                    return new NotFoundResult();
                }

                if (user != null)
                {
                    
                    user.Assignments = await m_assignmentsForUserData.GetAssignmentsForUser(user.RowKey).ToListAsync();
                    return new OkObjectResult(user);
                }
                else { return new NotFoundResult(); }
            }
        }
    }
}
