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
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "users/{userid:int?}")] HttpRequest req,
            ILogger log,
            int? userid)
        {
            if (userid != null)
            {
                var user = await m_userData.GetUser(userid.Value);
                if(user != null)
                {
                    return new OkObjectResult(user);
                }
            }
            else
            {
                var users = await m_userData.GetUsers();
                return new OkObjectResult(users);
            }

            return new BadRequestResult();
        }
    }
}
