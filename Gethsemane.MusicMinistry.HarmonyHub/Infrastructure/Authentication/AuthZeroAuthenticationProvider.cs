using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient.Results;
using Microsoft.Extensions.Configuration;

namespace Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Authentication;

public class AuthZeroClient : IAuthZeroClient
{
    private readonly IConfiguration _configuration;
    private readonly Auth0Client _client;
    private string? _name;
    private string? _email;

    public AuthZeroClient(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _configuration = configuration;

        _client = new Auth0Client(
            new Auth0ClientOptions
            {
                Domain = _configuration["Auth0:Domain"],
                ClientId = _configuration["Auth0:ClientId"],
                RedirectUri = _configuration["Auth0:RedirectUri"],
                PostLogoutRedirectUri = _configuration["Auth0:RedirectUri"],
                Scope = "openid profile email api offline_access"
            });
    }

    public async Task<LoginResult> LoginAsync(CancellationToken ct)
    {
        var loginResult = await _client.LoginAsync(
            cancellationToken: ct,
            extraParameters: new
            {
                audience = _configuration["Auth0:Audience"]
            });

        var user = loginResult.User;
        _name = user.FindFirst(c => c.Type == "name")?.Value;
        _email = user.FindFirst(c => c.Type == "email")?.Value;

        return loginResult;
    }

    public async Task<BrowserResultType> LogoutAsync(CancellationToken ct)
    {
        var logoutResult = await _client.LogoutAsync(cancellationToken: ct);
        _name = "Logged out";
        _email = "Please sign in";

        return logoutResult;
    }

    public async Task<RefreshTokenResult> RefreshTokenAsync(string token,
        CancellationToken ct)
    {
        var refreshResult = await _client.RefreshTokenAsync(token, ct);

        if (refreshResult.IsError)
        {
            _name = "Logged out";
            _email = "Please sign in";
        }

        return refreshResult;
    }

    public string? GetCurrentName()
    {
        return _name;
    }

    public string? GetCurrentEmail()
    {
        return _email;
    }
}
