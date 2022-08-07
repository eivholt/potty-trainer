using System;

namespace Data.TableEntities
{
    public class AssignmentForUserEntity : TableEntityBase
    {
        public static string PartitionKeyName = "assignmentforuser";
        public const string AssignmentRowKeyColumnName = "AssignmentRowKey";
        public const string UserRowKeyColumnName = "UserRowKey";

        public AssignmentForUserEntity(string rowKey) : base(PartitionKeyName, rowKey) { }

        public AssignmentForUserEntity() { }

        public string AssignmentRowKey { get; set; }
        public string UserRowKey { get; set; }

        public static AssignmentForUserEntity GetEntity(Assignment assignment, User user)
        {
            var assignmentForUserEntity = new AssignmentForUserEntity(Guid.NewGuid().ToString())
            {
                AssignmentRowKey = assignment.RowKey,
                UserRowKey = user.RowKey
            };

            return assignmentForUserEntity;
        }
    }
}