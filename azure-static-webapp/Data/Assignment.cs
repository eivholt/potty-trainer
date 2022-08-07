using Azure;
using Data.TableEntities;
using System;

namespace Data;

public class Assignment : DataModel
{
    public Assignment() : base()
    {

    }
    public Assignment(string rowKey, string partitionKey, DateTimeOffset? timestamp) : base(rowKey, partitionKey, timestamp)
    {
    }

    public Assignment(string rowKey) : base(rowKey, AssignmentEntity.PartitionKeyName)
    {
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public bool OncePerDay { get; set; }
    public int Weight { get; set; }
    public string Emoji { get; set; }
    public string EmojiModifier { get; set; }
}
