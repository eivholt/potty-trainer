using Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IAssignmentData
    {
        Task CompleteAssignment(string assignmentId, string userId, DateTime timeCompleted);
        Task<Assignment> GetUserAssignment(string assignmentId);
        IAsyncEnumerable<Assignment> GetUserAssignments();
    }
}
