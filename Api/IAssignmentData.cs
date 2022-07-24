using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public interface IAssignmentData
    {
        Task<Assignment> GetUserAssignment(string assignmentId);
        IAsyncEnumerable<Assignment> GetUserAssignments();
    }
}
