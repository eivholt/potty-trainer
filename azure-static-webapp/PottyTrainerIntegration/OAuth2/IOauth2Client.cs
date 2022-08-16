using Data;

namespace PottyTrainerIntegration.OAuth2
{
    public interface IOauth2Client
    {
        Task<UserAuth> GetAndStoreAccessToken(string code, string state);
        Task<UserAuth> RefreshAccessTokenAndStore(string refreshToken);
    }
}