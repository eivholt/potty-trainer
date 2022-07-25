using Azure;
using System;

namespace Data
{
    public abstract class DataModel
    {
        public DataModel()
        {

        }
        public DataModel(string rowKey, string partitionKey, DateTimeOffset? timestamp)
        {
            RowKey = rowKey;
            PartitionKey = partitionKey;
            Timestamp = timestamp;
        }

        public DataModel(string rowKey, string partitionKey)
        {
            RowKey = rowKey;
            PartitionKey = partitionKey;
        }

        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
