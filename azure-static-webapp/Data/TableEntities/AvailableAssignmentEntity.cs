using System;

namespace Data.TableEntities
{
    public class AvailableAssignmentEntity : TableEntityBase
    {
        public static string PartitionKeyName = "availableassignment";

        public AvailableAssignmentEntity(string rowKey) : base(PartitionKeyName, rowKey) { }

        public AvailableAssignmentEntity() { }

        public string AssignmentRowKey { get; set; }
        public string Name { get; set; }
        public string System { get; set; }
        public DateTime TimePosted { get; set; }

        public static AvailableAssignmentEntity GetEntity(string assignmentId, string name, string system, DateTime timePosted)
        {
            var availableAssignmentEntity = new AvailableAssignmentEntity(Guid.NewGuid().ToString().ToUpper())
            {
                AssignmentRowKey = assignmentId.ToUpper(),
                Name = name,
                System = system,
                TimePosted = timePosted
            };

            return availableAssignmentEntity;
        }

        public static AvailableAssignment FromEntity(AvailableAssignmentEntity availableAssignment, AssignmentEntity assignmentEntity)
        {
            return new AvailableAssignment(availableAssignment.RowKey)
            {
                Assignment = AssignmentEntity.FromEntity(assignmentEntity),
                AssignmentRowKey = assignmentEntity.RowKey,
                TimePosted = availableAssignment.TimePosted
            };
        }
    }
}
