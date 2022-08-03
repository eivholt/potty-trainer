using Azure.Data.Tables;
using Data;
using Data.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public AssignmentsForUserTable(IAssignmentData assignmentData)
        {
            m_assignmentData = assignmentData;
        }

        private IAssignmentData m_assignmentData { get; }

        public async IAsyncEnumerable<Assignment> GetAssignmentsForUser(string userId)
        {
            var assignmentsForUserQuery = m_assignmentsForUserTableClient.QueryAsync<AssignmentForUserEntity>(e => e.UserRowKey.Equals(userId)); //filter: "e => e.UserRowKey == 'userId'"
            var assignmentsForUserCompletedToday = m_assignmentData.GetCompletedAssignmentsToday(userId).SelectAwait(async a => await Task.Run(() => a.AssignmentRowKey));

            var assignmentsIds = assignmentsForUserQuery.SelectAwait(async a => await Task.Run(() => a.AssignmentRowKey));

            var assignmentsQuery = m_assigmentTableClient.QueryAsync<AssignmentEntity>();
            await foreach (var assignmentsResult in assignmentsQuery)
            {
                if (await assignmentsIds.AnyAwaitAsync(async a => await Task.Run(() => a.Equals(assignmentsResult.RowKey))) &&
                    !(assignmentsResult.OncePerDay &&
                        await assignmentsForUserCompletedToday.ContainsAsync(assignmentsResult.RowKey))
                    )
                {
                    yield return AssignmentEntity.FromEntity(assignmentsResult);
                }
            }
        }
    }
}