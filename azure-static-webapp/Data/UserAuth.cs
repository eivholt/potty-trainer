using System;

namespace Data
{
    public class UserAuth : DataModel
    {
        public UserAuth() : base() { }
        public UserAuth(string rowKey, string partitionKey) : base(rowKey, partitionKey) { }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public string Scope { get; set; }
        public string SystemUserId { get; set; }
        public string TokenType { get; set; }
    }
}
