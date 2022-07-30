﻿using Azure.Data.Tables;
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

        public async IAsyncEnumerable<Assignment> GetUserAssignments()
        {
            var assigmentQuery = m_assigmentTableClient.QueryAsync<AssignmentEntity>();
            await foreach (var assignmentResult in assigmentQuery)
            {
                yield return AssignmentEntity.FromEntity(assignmentResult);
            }
        }

        public async Task<int> CompleteAssignment(string assignmentId, string userId, DateTime timeCompleted)
        {
            var assignment = await GetUserAssignment(assignmentId);

            await m_completedAssigmentTableClient.AddEntityAsync<CompletedAssignmentEntity>(CompletedAssignmentEntity.GetEntity(assignment.RowKey, userId, timeCompleted, assignment.Weight));

            var completedAssignmentsForUserQuery = m_completedAssigmentTableClient.QueryAsync<CompletedAssignmentEntity>(e => e.UserRowKey.Equals(userId));

            int xpSum = 0;
            await foreach(var completedAssignment in completedAssignmentsForUserQuery)
            {
                xpSum += completedAssignment.XP;
            }

            return xpSum;
        }

        public async IAsyncEnumerable<CompletedAssignment> GetCompletedAssignmentsToday(string userId)
        {
            var completedAssignmentsForUserTodayQuery = m_completedAssigmentTableClient.QueryAsync<CompletedAssignmentEntity>(
            e =>
            e.UserRowKey.Equals(userId) &&
            e.TimeCompleted >= DateTime.UtcNow.Date);
            await foreach(var completedAssignment in completedAssignmentsForUserTodayQuery)
            {
                var assigmentResponse = await m_assigmentTableClient.GetEntityAsync<AssignmentEntity>(AssignmentEntity.PartitionKeyName, completedAssignment.AssignmentRowKey);

                yield return CompletedAssignmentEntity.FromEntity(completedAssignment, assigmentResponse.Value);
            }
        }
    }
}
