using System;

namespace Data.TableEntities
{
    public class UserAuthEntity : TableEntityBase
    {
        public static string WithingsSystemPartitionKeyName = "withings";
        public UserAuthEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }

        public UserAuthEntity() { }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public string Scope { get; set; }
        public string SystemUserId { get; set; }
        public string TokenType { get; set; }

        public static UserAuth FromEntity(UserAuthEntity userAuthEntity)
        {
            return new UserAuth(userAuthEntity.RowKey, userAuthEntity.PartitionKey)
            {
                AccessToken = userAuthEntity.AccessToken,
                RefreshToken = userAuthEntity.RefreshToken,
                Expires = userAuthEntity.Expires,
                Scope = userAuthEntity.Scope,
                SystemUserId = userAuthEntity.SystemUserId,
                TokenType = userAuthEntity.TokenType
            };
        }
    }
}