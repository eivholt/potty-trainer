using System.Collections.Generic;

namespace Data
{
    public class User
    {
        public User()
        {
            Assignments = new List<Assignment>();
            CompletedAssignments = new List<CompletedAssignment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public List<Assignment> Assignments { get; set; }

        public List<CompletedAssignment> CompletedAssignments {get; set;}
    }
}
