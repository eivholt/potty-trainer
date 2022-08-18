using Azure;
using Azure.Data.Tables;
using System;

namespace Data.TableEntities
{
    public abstract class TableEntityBase : ITableEntity
    {
        public TableEntityBase()
        {

        }
        
        public TableEntityBase(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public TableEntityBase(string partitionKey, string rowKey, DateTimeOffset timestamp)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Timestamp = timestamp;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
