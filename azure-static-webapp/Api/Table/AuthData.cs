using Azure.Data.Tables;
using Data;
using Data.TableEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Table
{
    public class AuthData : IAuthData
    {
        private readonly ILogger m_logger;
        private static string m_storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        private static string m_pottytrainerTableUserAuth = "userauth";
        private static TableServiceClient m_tableServiceClient = new TableServiceClient(m_storageConnectionString);
        private TableClient m_userAuthTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableUserAuth);

        public AuthData(ILoggerFactory loggerFactory)
        {
            m_logger = loggerFactory.CreateLogger<AuthData>();
        }

        public async Task<UserAuth> SaveAccessToken(
            string userKey, 
            string system, 
            string systemUserId,
            string accessToken, 
            string refreshToken, 
            DateTime expires, 
            string scope,
            string tokenType)
        {
            var userAuthEntity = new UserAuthEntity(system, userKey)
            {
                SystemUserId = systemUserId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Scope = scope,
                TokenType = tokenType,
                Expires = expires
            };

            var upserResponse = await m_userAuthTableClient.UpsertEntityAsync(userAuthEntity);
            if (upserResponse.IsError)
            {
                throw new InvalidOperationException("SaveAccessToken - UpserEntity failed.");
            }

            return UserAuthEntity.FromEntity(userAuthEntity);
        }

        public async Task<UserAuth> GetUserAuth(string userKey, string system)
        {
            return UserAuthEntity.FromEntity(await m_userAuthTableClient.GetEntityAsync<UserAuthEntity>(system, userKey));
        }

        public async Task<UserAuth> GetUserAuthBySystemUserId(string systemUserId, string system)
        {
            var userAuthBySystemUserIdQuery = m_userAuthTableClient.QueryAsync<UserAuthEntity>(u => u.PartitionKey.Equals(system) && u.SystemUserId.Equals(systemUserId));

            var users = new List<UserAuthEntity>();
            await foreach(var userAuthEntity in userAuthBySystemUserIdQuery)
            {
                users.Add(userAuthEntity);
            }

            if(users.Count == 1)
            {
                return UserAuthEntity.FromEntity(users.First());
            }
            else
            {
                throw new InvalidOperationException($"GetUserAuthBySystemUserId: UserAuth count: {users.Count}");
            }
        }

        public async Task<UserAuth> RefreshAccessToken(string systemUserId, string system, string newAccessToken, string newRefreshToken, DateTime expires, string scope, string tokenType)
        {
            var expiredUserAuth = await GetUserAuthBySystemUserId(systemUserId, system);
            expiredUserAuth.AccessToken = newAccessToken;
            expiredUserAuth.RefreshToken = newRefreshToken;
            expiredUserAuth.Expires = expires;
            expiredUserAuth.Scope = scope;
            expiredUserAuth.TokenType = tokenType;

            var updateUserAuthResponse = await m_userAuthTableClient.UpdateEntityAsync<UserAuthEntity>(UserAuthEntity.GetEntity(expiredUserAuth), Azure.ETag.All, TableUpdateMode.Merge);
            if (updateUserAuthResponse.IsError)
            {
                m_logger.LogError($"RefreshAccessToken failed: {updateUserAuthResponse.ReasonPhrase}", updateUserAuthResponse);
                throw new InvalidOperationException($"RefreshAccessToken failed: {updateUserAuthResponse.ReasonPhrase}");
            }

            return UserAuthEntity.FromEntity(await m_userAuthTableClient.GetEntityAsync<UserAuthEntity>(system, systemUserId));
        }
    }
}
