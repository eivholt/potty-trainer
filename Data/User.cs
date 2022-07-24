using System.Collections.Generic;

namespace Data
{
    public class User
    {
        public User()
        {
            //Assignments = new List<Assignment>();
            //CompletedAssignments = new List<CompletedAssignment>();
        }
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public List<Assignment> Assignments { get; set; }
        //public List<CompletedAssignment> CompletedAssignments {get; set;}
        public int Goal { get; set; }
        public int XP { get; set; }
        //public List<int> Badges { get; set; }
    }
}
