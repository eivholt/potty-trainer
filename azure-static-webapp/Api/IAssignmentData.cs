using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IAssignmentData
    {
        Task<int> CompleteAssignment(string assignmentId, string userId);
        Task<int> CalculateXp(string userId);
        IAsyncEnumerable<CompletedAssignment> GetCompletedAssignmentsToday(string userId);
        IAsyncEnumerable<CompletedAssignment> GetCompletedAssignmentsYesterday(string userId);
        Task<Assignment> GetUserAssignment(string assignmentId);
        Task<bool> DeleteCompletedAssignment(string userId, string completedAssignmentId);
        IAsyncEnumerable<AvailableAssignment> GetAvailableAssignments();
        Task<AvailableAssignment> AddAvailableAssignment(string assignmentId, string system);
        Task<int> CompleteAvailableAssignment(string availableAssignmentId, string userId);
        IAsyncEnumerable<AvailableAssignment> AvailableAssignmentTypeTodayExists(string assignmentId);
    }
}
