using Azure;
using Data.TableEntities;
using System;
using System.Collections.Generic;

namespace Data
{
    public class User : DataModel
    {
        public User() : base() { }
        public User(string rowKey) : base(rowKey, UserEntity.PartitionKeyName, null) { }

        public User(string rowKey, DateTimeOffset? timestamp) : base(rowKey, UserEntity.PartitionKeyName, timestamp) { }

        public string Name { get; set; }
        public string Avatar { get; set; }
        public List<Assignment> Assignments { get; set; }
        //public List<CompletedAssignment> CompletedAssignments {get; set;}
        public int Goal { get; set; }
        public int XP { get; set; }
    }
}
