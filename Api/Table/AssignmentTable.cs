using Azure.Data.Tables;
using Data;
using Data.TableEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Table
{
    public class AssignmentTable : IAssignmentData
    {
        private static string m_storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        private static string m_pottytrainerTableAssignments = "assignments";
        private static string m_pottytrainerTableCompletedAssignments = "completedassignments";
        private static TableServiceClient m_tableServiceClient = new TableServiceClient(m_storageConnectionString);
        private TableClient m_assigmentTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableAssignments);
        private TableClient m_completedAssigmentTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableCompletedAssignments);

        public async Task<Assignment> GetUserAssignment(string assignmentId)
        {
            return AssignmentEntity.FromEntity(await m_assigmentTableClient.GetEntityAsync<AssignmentEntity>(AssignmentEntity.PartitionKeyName, assignmentId.ToUpper()));
        }

        public async Task<int> CompleteAssignment(string assignmentId, string userId)
        {
            var assignment = await GetUserAssignment(assignmentId);

            await m_completedAssigmentTableClient.AddEntityAsync(CompletedAssignmentEntity.GetEntity(assignment.RowKey, userId, assignment.Weight, assignment.Name, DateTime.UtcNow));

            var completedAssignmentsForUserQuery = m_completedAssigmentTableClient.QueryAsync<CompletedAssignmentEntity>(e => e.UserRowKey.Equals(userId));

            int xpSum = 0;
            await foreach(var completedAssignment in completedAssignmentsForUserQuery)
            {
                xpSum += completedAssignment.XP;
            }

            return xpSum;
        }

        public async Task<int> CalculateXp(string userId)
        {
            var completedAssignmentsForUserQuery = m_completedAssigmentTableClient.QueryAsync<CompletedAssignmentEntity>(e => e.UserRowKey.Equals(userId));

            int xpSum = 0;
            await foreach (var completedAssignment in completedAssignmentsForUserQuery)
            {
                xpSum += completedAssignment.XP;
            }

            return xpSum;
        }

        public async IAsyncEnumerable<CompletedAssignment> GetCompletedAssignmentsToday(string userId)
        {
            await foreach(var completedAssignment in GetCompletedAssignmentsTimeSpan(userId, DateTime.UtcNow.Date, DateTime.UtcNow))
            {
                yield return completedAssignment;
            }
        }

        public async IAsyncEnumerable<CompletedAssignment> GetCompletedAssignmentsYesterday(string userId)
        {
            await foreach (var completedAssignment in GetCompletedAssignmentsTimeSpan(userId, DateTime.UtcNow.AddDays(-1).Date, DateTime.UtcNow.Date))
            {
                yield return completedAssignment;
            }
        }

        private async IAsyncEnumerable<CompletedAssignment> GetCompletedAssignmentsTimeSpan(string userId, DateTime fromDateInclusive, DateTime toDateInclusive)
        {
            var completedAssignmentsForUserTodayQuery = m_completedAssigmentTableClient.QueryAsync<CompletedAssignmentEntity>(
            e =>
            e.UserRowKey.Equals(userId) &&
            e.TimeCompleted >= fromDateInclusive &&
            e.TimeCompleted <= toDateInclusive);
            await foreach(var completedAssignment in completedAssignmentsForUserTodayQuery)
            {
                var assigmentResponse = await m_assigmentTableClient.GetEntityAsync<AssignmentEntity>(AssignmentEntity.PartitionKeyName, completedAssignment.AssignmentRowKey);

                yield return CompletedAssignmentEntity.FromEntity(completedAssignment, assigmentResponse.Value);
            }
        }

        public async Task<bool> DeleteCompletedAssignment(string userId, string completedAssignmentId)
        {
            var deleteCompletedAssignmentResult = await m_completedAssigmentTableClient.DeleteEntityAsync(CompletedAssignmentEntity.PartitionKeyName, completedAssignmentId);
            return !deleteCompletedAssignmentResult.IsError;
        }
    }
}
