using Data;
using System.Collections.Generic;

namespace Api
{
    public interface IAssignmentsForUser
    {
        IAsyncEnumerable<Assignment> GetAssignmentsForUser(string userId);
    }
}
