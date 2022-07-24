using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IUserData
    {
        Task<User> GetUser(string rowKey);
        IAsyncEnumerable<User> GetUsers();
    }
}
