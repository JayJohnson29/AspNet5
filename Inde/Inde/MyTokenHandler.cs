using Inde.Service;


public class MyTokenHandler : DelegatingHandler
{
    private readonly IMyTokenService _myTokenService;

    public MyTokenHandler(IMyTokenService myTokenService)
    {
        _myTokenService = myTokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _myTokenService.GetAccessTokenAsync();
        request.Headers.Add("UserToken", accessToken.ToString());
        return await base.SendAsync(request, cancellationToken);
    }
}