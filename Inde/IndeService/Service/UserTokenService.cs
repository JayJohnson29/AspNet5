
using IndeService.Repository;

namespace IndeService.Service;

public class UserTokenService : IUserTokenService
{
    private readonly ILogger<UserTokenService> _logger;
    private readonly IMcLoginRepository _mcLoginRepository;
    private readonly IMcMemoryCache<McUser> _mcMemoryCache;

    public UserTokenService(ILogger<UserTokenService> logger, IMcLoginRepository mcLoginRepository, IMcMemoryCache<McUser> mcMemoryCache)
    {
        _logger = logger;
        _mcLoginRepository = mcLoginRepository;
        _mcMemoryCache = mcMemoryCache;
    }

    public async Task<Guid> GetAccessTokenAsync()
    {

        var cachedLogin = await _mcMemoryCache.Get("McUserKey");
        if (cachedLogin != null && cachedLogin.UserTokenExpiration > DateTime.Now.AddMinutes(5))
        {
            return cachedLogin.UserToken;
        }

        var login = await _mcLoginRepository.Login();

        await _mcMemoryCache.Set("McUserKey", login.Data);

        return login.Data.UserToken;
    }
}
