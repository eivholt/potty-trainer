using Api;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;

namespace PottyTrainerIntegration
{
    internal class UserSubscribeSystemPost
    {
        private readonly ILogger m_logger;
        private readonly IAuthData m_authData;

        public UserSubscribeSystemPost(ILoggerFactory loggerFactory, IAuthData authData)
        {
            m_logger = loggerFactory.CreateLogger<CompleteAssignmentForUser>();
            m_authData = authData;
        }

        [Function("UserSubscribeSystemPost")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Users/{userid}/Subscribe/{system}")] HttpRequestData req,
            string userid,
            string system)
        {
            m_logger.LogInformation($"UserSubscribeSystemPost: {userid} - {system}");
            try 
            { 
                var requestBodyJsonDom = await JsonSerializer.DeserializeAsync<JsonObject>(req.Body);
                m_logger.LogInformation($"UserSubscribeSystemPost:", requestBodyJsonDom.ToJsonString());

                var userAuth = await m_authData.GetUserAuth(userid, system);



                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(userAuth);
                return response;
            } 
            catch
            {
                m_logger.LogError($"UserSubscribeSystemPost");
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }
        }
    }
}