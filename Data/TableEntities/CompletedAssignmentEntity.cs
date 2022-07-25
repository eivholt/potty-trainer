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
        public DateTime TimeCompleted { get; set; }
        public int XP { get; set; }

        public static CompletedAssignmentEntity GetEntity(string assignmentId, string userId, DateTime timeCompleted, int xp)
        {
            var completedAssignmentEntity = new CompletedAssignmentEntity(Guid.NewGuid().ToString())
            {
                AssignmentRowKey = assignmentId,
                UserRowKey = userId,
                TimeCompleted = timeCompleted,
                XP = xp
            };

            return completedAssignmentEntity;
        }
    }
}