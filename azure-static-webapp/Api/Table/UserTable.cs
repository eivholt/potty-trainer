﻿using Azure.Data.Tables;
using Data;
using Data.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<User> GetUserFromDeviceId(string deviceType, string deviceId)
        {
            var userByDeviceQuery = m_userTableClient.QueryAsync<UserEntity>(e => e.DosetteDeviceId.Equals(deviceId));

            List<UserEntity> usersWithSelectedDeviceId = new List<UserEntity>();
            
            await foreach(var user in userByDeviceQuery)
            {
                usersWithSelectedDeviceId.Add(user);
            }
            
            if(usersWithSelectedDeviceId.Count > 1)
            {
                throw new InvalidOperationException($"Non-unique use of device id: {deviceType}, {deviceId}");
            }

            if(usersWithSelectedDeviceId.Count == 0)
            {
                throw new InvalidOperationException($"Device id not found: {deviceType}, {deviceId}");
            }

            return UserEntity.FromEntity(usersWithSelectedDeviceId.First());
        }

        public async Task<User> UpdateXp(string userId, int xp)
        {
            var userEntityToUpdate = await m_userTableClient.GetEntityAsync<UserEntity>(UserEntity.PartitionKeyName, userId.ToUpper());
            userEntityToUpdate.Value.XP = xp;
            await m_userTableClient.UpdateEntityAsync<UserEntity>(userEntityToUpdate, userEntityToUpdate.Value.ETag, TableUpdateMode.Merge);
            var userEntityUpdated = await m_userTableClient.GetEntityAsync<UserEntity>(UserEntity.PartitionKeyName, userId.ToUpper());
            return UserEntity.FromEntity(userEntityUpdated);
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
