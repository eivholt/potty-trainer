using System;

namespace Data;

public class Assignment
{
    public Assignment()
    {
        Emoji = "twa-pile-of-poo";
        EmojiModifier = string.Empty;
    }

    public string RowKey { get; set; }
    public string PartitionKey { get; set; }

    public DateTimeOffset Timestamp { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool OncePerDay { get; set; }
    public int Weight { get; set; }
    public string Emoji { get; set; }
    public string EmojiModifier { get; set; }
}
