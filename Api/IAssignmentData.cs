using Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IAssignmentData
    {
        Task<int> CompleteAssignment(string assignmentId, string userId, DateTime timeCompleted);
        IAsyncEnumerable<CompletedAssignment> GetCompletedAssignmentsToday(string userId);
        Task<Assignment> GetUserAssignment(string assignmentId);
        IAsyncEnumerable<Assignment> GetUserAssignments();
    }
}
