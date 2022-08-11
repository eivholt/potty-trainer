using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public interface IAuthData
    {
        Task<bool> SaveAccessToken(string userKey, string systemUserId, string system, string accessToken, string refreshToken, DateTime expires, string scope, string tokenType);
    }
}
