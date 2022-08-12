using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public interface IAuthData
    {
        Task<UserAuth> GetUserAuth(string userKey, string system);
        Task<bool> SaveAccessToken(string userKey, string system, string systemUserId, string accessToken, string refreshToken, DateTime expires, string scope, string tokenType);
    }
}
