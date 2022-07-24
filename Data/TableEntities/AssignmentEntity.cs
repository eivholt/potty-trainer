using Azure.Data.Tables;
using System;

namespace Data.TableEntities
{
    public static class AssignmentEntity
    {
        public const string PartitionKey = "assignment";
        public static TableEntity GetEntity(Assignment assignment)
        {
            var assignmentEntity = new TableEntity(PartitionKey, assignment.RowKey) 
            {
                { nameof(Assignment.Name), assignment.Name },
                { nameof(Assignment.Description), assignment.Description },
                { nameof(Assignment.Emoji), assignment.Emoji },
                { nameof(Assignment.EmojiModifier), assignment.EmojiModifier },
                { nameof(Assignment.OncePerDay), assignment.OncePerDay },
                { nameof(Assignment.Weight), assignment.Weight }
            };
            
            return assignmentEntity;
        }

        public static Assignment FromEntity(TableEntity assignmentEntity)
        {
            return new Assignment
            {
                PartitionKey = assignmentEntity.PartitionKey,
                RowKey = assignmentEntity.RowKey,
                Timestamp = assignmentEntity.Timestamp.Value,
                Emoji = assignmentEntity.GetString(nameof(Assignment.Emoji)),
                EmojiModifier = assignmentEntity.GetString(nameof(Assignment.EmojiModifier)),
                Name = assignmentEntity.GetString(nameof(Assignment.Name)),
                Description = assignmentEntity.GetString(nameof(Assignment.Description)),
                OncePerDay = assignmentEntity.GetBoolean(nameof(Assignment.OncePerDay)).Value,
                Weight = assignmentEntity.GetInt32(nameof(Assignment.Weight)).Value
            };
        }
    }
}
