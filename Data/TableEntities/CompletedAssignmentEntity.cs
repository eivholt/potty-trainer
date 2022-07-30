using System;

namespace Data.TableEntities
{
    public class CompletedAssignmentEntity : TableEntityBase
    {
        public static string PartitionKeyName = "completedassignment";

        public CompletedAssignmentEntity(string rowKey) : base(PartitionKeyName, rowKey) { }

        public CompletedAssignmentEntity() { }

        public string AssignmentRowKey { get; set; }
        public string UserRowKey { get; set; }
        public int XP { get; set; }
        public string Name { get; set; }

        public static CompletedAssignmentEntity GetEntity(string assignmentId, string userId, int xp, string name)
        {
            var completedAssignmentEntity = new CompletedAssignmentEntity(Guid.NewGuid().ToString())
            {
                AssignmentRowKey = assignmentId,
                UserRowKey = userId,
                XP = xp,
                Name = name
            };

            return completedAssignmentEntity;
        }

        public static CompletedAssignment FromEntity(CompletedAssignmentEntity completedAssignment, AssignmentEntity assignmentEntity)
        {
            return new CompletedAssignment(completedAssignment.RowKey.ToUpper(), completedAssignment.PartitionKey, completedAssignment.Timestamp)
            {
                Assignment = AssignmentEntity.FromEntity(assignmentEntity),
                AssignmentRowKey = completedAssignment.AssignmentRowKey,
                UserRowKey = completedAssignment.UserRowKey,
                XP = completedAssignment.XP
            };
        }
    }
}