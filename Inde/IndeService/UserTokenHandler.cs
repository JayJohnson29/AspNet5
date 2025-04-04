using IndeService.Service;


public class UserTokenHandler : DelegatingHandler
{
    private readonly IUserTokenService _userTokenService;

    public UserTokenHandler(IUserTokenService userTokenService)
    {
        _userTokenService = userTokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _userTokenService.GetAccessTokenAsync();
        request.Headers.Add("UserToken", accessToken.ToString());
        return await base.SendAsync(request, cancellationToken);
    }
}