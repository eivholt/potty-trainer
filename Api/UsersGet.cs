using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using User = Data.User;

namespace Api
{
    public class UsersGet
    {
        private readonly IUserData m_userData;

        public UsersGet(IUserData userData)
        {
            m_userData = userData;
        }

        [FunctionName("UsersGet")]
        public IActionResult UsersGetStreaming(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req,
            ILogger log
            )
        {
            return new OkObjectResult(m_userData.GetUsers());
        }

        [FunctionName("UserGet")]
        public async Task<IActionResult> UserGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userid}")] HttpRequest req,
            ILogger log,
            string userId)
        {
            if (!Guid.TryParse(userId, out _))
            {
                return new BadRequestResult();
            } 
            else 
            {
                User user;
                try { user = await m_userData.GetUser(userId); }
                catch (Azure.RequestFailedException)
                {
                    return new NotFoundResult();
                }
                
                if (user != null)
                {
                    return new OkObjectResult(user);
                }
                else { return new NotFoundResult(); }
            }
        }
    }
}