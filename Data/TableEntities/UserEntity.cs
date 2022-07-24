using Azure.Data.Tables;
namespace Data.TableEntities
{
    public static class UserEntity
    {
        public const string PartitionKey = "user";

        public static TableEntity GetEntity(User user)
        {
            var userEntity = new TableEntity(PartitionKey, user.RowKey) 
            {
                { nameof(User.Name), user.Name },
                { nameof(User.Avatar), user.Avatar },
                { nameof(User.Goal), user.Goal },
                { nameof(User.XP), user.XP }
            };

            return userEntity;
        }

        public static User FromEntity(TableEntity userEntity)
        {
            return new User
            {
                PartitionKey = userEntity.PartitionKey,
                RowKey = userEntity.RowKey,
                Avatar = userEntity.GetString(nameof(User.Avatar)),
                Name = userEntity.GetString(nameof(User.Name)),
                Goal = userEntity.GetInt32(nameof(User.Goal)).Value,
                XP = userEntity.GetInt32(nameof(User.XP)).Value,
            };
        }
    }
}
