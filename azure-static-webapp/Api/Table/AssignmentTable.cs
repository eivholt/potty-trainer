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
        private static string m_pottytrainerTableAvailableAssignments = "availableassignments";
        private static TableServiceClient m_tableServiceClient = new TableServiceClient(m_storageConnectionString);
        private TableClient m_assigmentTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableAssignments);
        private TableClient m_completedAssigmentTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableCompletedAssignments);
        private TableClient m_availableAssigmentTableClient = m_tableServiceClient.GetTableClient(m_pottytrainerTableAvailableAssignments);

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
            var completedAssignmentsForUserTimespanQuery = m_completedAssigmentTableClient.QueryAsync<CompletedAssignmentEntity>(
            e =>
            e.UserRowKey.Equals(userId) &&
            e.TimeCompleted >= fromDateInclusive &&
            e.TimeCompleted <= toDateInclusive);

            var allAssignmentsResponse = m_assigmentTableClient.QueryAsync<AssignmentEntity>();
            var allAssignments = new List<AssignmentEntity>();

            await foreach(var assignment in allAssignmentsResponse)
            {
                allAssignments.Add(assignment);
            }

            await foreach(var completedAssignment in completedAssignmentsForUserTimespanQuery)
            {
                yield return CompletedAssignmentEntity.FromEntity(completedAssignment, allAssignments.Find(a => a.RowKey.Equals(completedAssignment.AssignmentRowKey)));
            }
        }

        public async Task<bool> DeleteCompletedAssignment(string userId, string completedAssignmentId)
        {
            var deleteCompletedAssignmentResult = await m_completedAssigmentTableClient.DeleteEntityAsync(CompletedAssignmentEntity.PartitionKeyName, completedAssignmentId);
            return !deleteCompletedAssignmentResult.IsError;
        }

        public async IAsyncEnumerable<AvailableAssignment> GetAvailableAssignments()
        {
            var availabledAssignmentsTimespanQuery = m_availableAssigmentTableClient.QueryAsync<AvailableAssignmentEntity>(e => e.TimePosted >= DateTime.UtcNow.Date);

            var allAssignmentsResponse = m_assigmentTableClient.QueryAsync<AssignmentEntity>();
            var allAssignments = new List<AssignmentEntity>();

            await foreach (var assignment in allAssignmentsResponse)
            {
                allAssignments.Add(assignment);
            }

            await foreach (var availableAssignment in availabledAssignmentsTimespanQuery)
            {
                yield return AvailableAssignmentEntity.FromEntity(availableAssignment, allAssignments.Find(a => a.RowKey.Equals(availableAssignment.AssignmentRowKey)));
            }
        }

        public async Task<AvailableAssignment> AddAvailableAssignment(string assignmentId, string system)
        {
            var assignment = await GetUserAssignment(assignmentId);

            var avaliableAssignment = AvailableAssignmentEntity.GetEntity(assignment.RowKey, assignment.Name, system, DateTime.UtcNow);
            await m_availableAssigmentTableClient.AddEntityAsync(avaliableAssignment);

            return AvailableAssignmentEntity.FromEntity(avaliableAssignment, AssignmentEntity.GetEntity(assignment));
        }

        public async Task<int> CompleteAvailableAssignment(string availableAssignmentId, string userId)
        {
            var availableAssignmentEntity = await m_availableAssigmentTableClient.GetEntityAsync<AvailableAssignmentEntity>(AvailableAssignmentEntity.PartitionKeyName, availableAssignmentId.ToUpper());
            var xpSum = await CompleteAssignment(availableAssignmentEntity.Value.AssignmentRowKey, userId);
            await m_availableAssigmentTableClient.DeleteEntityAsync(AvailableAssignmentEntity.PartitionKeyName, availableAssignmentId.ToUpper());

            return xpSum;
        }

        public async IAsyncEnumerable<AvailableAssignment> AvailableAssignmentTypeTodayExists(string assignmentId)
        {
            var availabledAssignmentsTimespanQuery = m_availableAssigmentTableClient.QueryAsync<AvailableAssignmentEntity>(
                e => e.AssignmentRowKey.Equals(assignmentId) && 
                e.TimePosted >= DateTime.UtcNow.Date);

            await foreach(var availableAssignmentEntity in availabledAssignmentsTimespanQuery)
            {
                yield return AvailableAssignmentEntity.FromEntity(availableAssignmentEntity, AssignmentEntity.GetEntity(await GetUserAssignment(availableAssignmentEntity.AssignmentRowKey)));
            }
        }
    }
}