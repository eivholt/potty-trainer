﻿using Data;
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
        Task<UserAuth> GetUserAuthBySystemUserId(string systemUserId, string system);
        Task<UserAuth> RefreshAccessToken(string userKey, string system, string newAccessToken, string newRefreshToken, DateTime expires, string scope, string tokenType);
        Task<UserAuth> SaveAccessToken(string userKey, string system, string systemUserId, string accessToken, string refreshToken, DateTime expires, string scope, string tokenType);
    }
}
