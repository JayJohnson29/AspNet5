
using Inde.Repository;
using Microsoft.AspNetCore.Http.Features;

namespace Inde.Service;

public class MyTokenService : IMyTokenService
{
    private readonly ILogger<MyTokenService> _logger;
    private readonly IMcLoginRepository _mcLoginRepository;
    private readonly IMcMemoryCache<McUser> _mcMemoryCache;

    public MyTokenService(ILogger<MyTokenService> logger, IMcLoginRepository mcLoginRepository, IMcMemoryCache<McUser> mcMemoryCache)
    {
        _logger = logger;
        _mcLoginRepository = mcLoginRepository;
        _mcMemoryCache = mcMemoryCache;
    }

    public async Task<Guid> GetAccessTokenAsync()
    {

        var cachedLogin = await _mcMemoryCache.Get("McUserKey");
        if (cachedLogin != null && cachedLogin.UserTokenExpiration > DateTime.Now)
        {
            return cachedLogin.UserToken;
        }

        var login = await _mcLoginRepository.Login();

        await _mcMemoryCache.Set("McUserKey", login.Data);

        return login.Data.UserToken;
    }
}
