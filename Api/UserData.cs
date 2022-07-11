using Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public interface IUserData
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
    }

    public class UserData : IUserData
    {
        private readonly List<User> m_users = new List<User>
        {
            new User
            {
                Id = 10,
                Name = "Lilly",
                Avatar = "Bilde av Lilly"
            },
            new User
            {
                Id = 20,
                Name = "Robin",
                Avatar = "Bilde av Robin"
            }
        };

        public Task<User> GetUser(int id)
        {
            var index = this.m_users.FindIndex(u => u.Id == id);
            return Task.FromResult(m_users[index]);
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return Task.FromResult(m_users.AsEnumerable());
        }
    }
}
