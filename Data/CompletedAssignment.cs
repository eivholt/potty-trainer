using Data.TableEntities;
using System;

namespace Data
{
    public class CompletedAssignment : DataModel
    {
        public CompletedAssignment() {}
        public CompletedAssignment(string rowKey, string partitionKey, DateTimeOffset? timestamp) : base(rowKey, partitionKey, timestamp) {}

        public CompletedAssignment(string rowKey) : base(rowKey, AssignmentEntity.PartitionKeyName) { }

        public Assignment Assignment { get; set; }

        public string AssignmentRowKey { get; set; }
        public string UserRowKey { get; set; }
        public int XP { get; set; }
    }
}
