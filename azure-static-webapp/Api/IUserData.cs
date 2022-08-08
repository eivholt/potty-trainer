using Azure;
using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public interface IUserData
    {
        Task<User> GetUser(string userId);
        Task<User> GetUserFromDeviceId(string deviceType, string deviceId);
        IAsyncEnumerable<User> GetUsers();
        Task<User> UpdateXp(string userId, int xp);
    }
}
