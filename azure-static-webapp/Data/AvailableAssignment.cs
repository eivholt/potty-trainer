using Data.TableEntities;
using System;

namespace Data
{
    public class AvailableAssignment : DataModel
    {
        public AvailableAssignment() { }
        public AvailableAssignment(string rowKey, string partitionKey, DateTimeOffset? timestamp) : base(rowKey, partitionKey, timestamp) { }

        public AvailableAssignment(string rowKey) : base(rowKey, AvailableAssignmentEntity.PartitionKeyName) { }

        public Assignment Assignment { get; set; }

        public string AssignmentRowKey { get; set; }

        public DateTime TimePosted { get; set; }
    }
}
