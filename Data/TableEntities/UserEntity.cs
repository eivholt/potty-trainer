using Azure;
using Azure.Data.Tables;
using System;

namespace Data.TableEntities
{
    public class UserEntity : TableEntityBase
    {
        public static string PartitionKeyName = "user";

        public UserEntity(string rowKey) : base(PartitionKeyName, rowKey)
        {
        }

        public UserEntity()
        {

        }

        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Goal { get; set; } = 0;
        public int XP { get; set; } = 0;

        public static UserEntity GetEntity(User user)
        {
            var userEntity = new UserEntity(user.RowKey) 
            {
                Name = user.Name,
                Avatar = user.Avatar,
                Goal = user.Goal,
                XP = user.XP
            };

            return userEntity;
        }

        public static User FromEntity(UserEntity userEntity)
        {
            return new User(userEntity.RowKey, userEntity.Timestamp)
            {
                Avatar = userEntity.Avatar,
                Name = userEntity.Name,
                Goal = userEntity.Goal,
                XP = userEntity.XP,
            };
        }
    }
}
