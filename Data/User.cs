using System.Collections.Generic;

namespace Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public List<Assignment> Assignments { get; }
    }
}
