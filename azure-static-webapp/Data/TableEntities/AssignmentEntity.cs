using Azure.Data.Tables;
using System;

namespace Data.TableEntities
{
    public class AssignmentEntity : TableEntityBase
    {
        public static string PartitionKeyName = "assignment";

        public AssignmentEntity() { }

        public AssignmentEntity(string rowKey) : base(PartitionKeyName, rowKey) { }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Emoji { get; set; }
        public string EmojiModifier { get; set; }
        public bool OncePerDay { get; set; }
        public int Weight { get; set; }

        public static AssignmentEntity GetEntity(Assignment assignment)
        {
            var assignmentEntity = new AssignmentEntity(assignment.RowKey) 
            {
                Name = assignment.Name,
                Description = assignment.Description,
                Emoji = assignment.Emoji,
                EmojiModifier = assignment.EmojiModifier,
                OncePerDay = assignment.OncePerDay,
                Weight = assignment.Weight
            };
            
            return assignmentEntity;
        }

        public static Assignment FromEntity(AssignmentEntity assignmentEntity)
        {
            return new Assignment(assignmentEntity.RowKey, PartitionKeyName, assignmentEntity.Timestamp)
            {
                Emoji = assignmentEntity.Emoji,
                EmojiModifier = assignmentEntity.EmojiModifier,
                Name = assignmentEntity.Name,
                Description = assignmentEntity.Description,
                OncePerDay = assignmentEntity.OncePerDay,
                Weight = assignmentEntity.Weight
            };
        }
    }
}