using Azure.Data.Tables;
using Data;
using Data.TableEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Table
{
    public class UserTable : IUserData
    {
        private static string m_storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        private static string m_pottytrainerTableUsers = "users";
        private static TableServiceClient m_tableServiceClient = new TableServiceClient(m_storageConnectionString);
        private TableClient m_userTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableUsers);

        public async Task<User> GetUser(string userId)
        {
            return UserEntity.FromEntity(await m_userTableClient.GetEntityAsync<UserEntity>(UserEntity.PartitionKeyName, userId.ToUpper()));
        }

        public async IAsyncEnumerable<User> GetUsers()
        {
            var userQuery = m_userTableClient.QueryAsync<UserEntity>();
            await foreach (var userResult in userQuery)
            {
                yield return UserEntity.FromEntity(userResult);
            }
        }
    }
}
