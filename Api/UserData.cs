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
        Task<User> GetUserWithAssignments(int id);
    }

    public class UserData : IUserData
    {
        private readonly List<Assignment> m_assignments = new List<Assignment>
        {
            new Assignment
            {
                Id = 10,
                Name = "Bæsj",
                Description = "Bæsje i do",
                OncePerDay = false,
                Weight = 5
            },
            new Assignment
            {
                Id = 11,
                Name = "Tiss",
                Description = "Tisse i do",
                OncePerDay = false,
                Weight = 2
            },
            new Assignment
            {
                Id = 20,
                Name = "Tannpuss morgen",
                Description = "Pusse tenner morgen",
                OncePerDay = true,
                Weight = 1
            },
            new Assignment
            {
                Id = 21,
                Name = "Tannpuss kveld",
                Description = "Pusse tenner kveld",
                OncePerDay = true,
                Weight = 1
            },
            new Assignment
            {
                Id = 30,
                Name = "Matte",
                Description = "Gjøre matteoppgave",
                OncePerDay = true,
                Weight = 5
            },
            new Assignment
            {
                Id = 31,
                Name = "Lese",
                Description = "Lese selv",
                OncePerDay = true,
                Weight = 5
            },
            new Assignment
            {
                Id = 40,
                Name = "Rydde rommet",
                Description = "Rydde rommet",
                OncePerDay = true,
                Weight = 7
            },
            new Assignment
            {
                Id = 41,
                Name = "Bære fatet",
                Description = "Bære middagsfatet til kjøkken",
                OncePerDay = true,
                Weight = 1
            }
        };

        private readonly List<User> m_users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Lilly",
                Avatar = "twa-girl"
            },
            new User
            {
                Id = 2,
                Name = "Robin",
                Avatar = "twa-boy"
            }
        };

        public Task<User> GetUser(int id)
        {
            var index = m_users.FindIndex(u => u.Id == id);
            return Task.FromResult(m_users[index]);
        }

        public Task<User> GetUserWithAssignments(int id)
        {
            var user = GetUser(id).Result;
            int[] assignmentIds = System.Array.Empty<int>();

            if(user.Id == 1) 
            {
                assignmentIds = new int[] { 20, 21, 30, 31, 40, 41 };
            } 
            else if (user.Id == 2)
            {
                assignmentIds = new int[] { 10, 11, 20, 21, 41 };
            }

            user.Assignments = new List<Assignment>(from userAssignment in m_assignments
                               where assignmentIds.Contains(userAssignment.Id)
                               select userAssignment);

            return Task.FromResult(user);
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return Task.FromResult(m_users.AsEnumerable());
        }
    }
}
