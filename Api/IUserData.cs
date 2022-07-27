using Azure;
using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IUserData
    {
        Task<User> GetUser(string userId);
        IAsyncEnumerable<User> GetUsers();
        Task<Response> UpdateXp(string userId, int xp);
    }
}
