using Azure.Data.Tables;
using Data;
using Data.TableEntities;
using System;
using System.Collections.Generic;

namespace Api.Table
{
    public class AssignmentsForUserTable : IAssignmentsForUser
    {
        private static string m_storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        private static string m_pottytrainerTableAssignmentsForUser = "assignmentsforuser";
        private static string m_pottytrainerTableAssignments = "assignments";
        private static TableServiceClient m_tableServiceClient = new TableServiceClient(m_storageConnectionString);
        private TableClient m_assignmentsForUserTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableAssignmentsForUser);
        private TableClient m_assigmentTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableAssignments);

        public async IAsyncEnumerable<Assignment> GetAssignmentsForUser(string userId)
        {
            var assignmentsForUserQuery = m_assignmentsForUserTableClient.QueryAsync<AssignmentForUserEntity>(e => e.UserRowKey.Equals(userId)); //filter: "e => e.UserRowKey == 'userId'"
            // select..
            List<string> assignmentsIds = new();
            await foreach (var assignmentsForUserResult in assignmentsForUserQuery)
            {
                assignmentsIds.Add(assignmentsForUserResult.AssignmentRowKey);
            }

            var assignmentsQuery = m_assigmentTableClient.QueryAsync<AssignmentEntity>();
            await foreach (var assignmentsResult in assignmentsQuery)
            {
                if (assignmentsIds.Contains(assignmentsResult.RowKey))
                {
                    yield return AssignmentEntity.FromEntity(assignmentsResult);
                }
            }
        }
    }
}